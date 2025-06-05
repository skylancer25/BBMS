using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using BBMS_1.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace BBMS_1.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IConfiguration _configuration;
        public DashboardController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IActionResult Index(int userId)
        {
            UserModel userModel = new UserModel();

            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DBConnection")))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("GetPatientDetailsByUserId", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@UserId", userId);

                SqlDataReader reader = cmd.ExecuteReader();

                // Read the result and assign values to the model
                if (reader.Read())
                {
                    userModel.patient_name = reader["usr_name"].ToString();
                    userModel.email = reader["email"].ToString();
                    userModel.blood_grp = reader["blood_grp_name"].ToString();
                    userModel.Address = reader["user_address"].ToString();
                    userModel.Pincode = reader["pincode"].ToString();  // Fetch Pincode
                    userModel.usertype = reader["user_type"].ToString(); // Fetch user type
                }
            }

            return View("~/Views/Home/dashboard.cshtml", userModel);

        }
    }
}
