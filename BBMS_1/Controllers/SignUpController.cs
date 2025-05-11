using BBMS_1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace BBMS_1.Controllers
{
    public class SignUpController : Controller
    {
        private readonly IConfiguration _configuration;

        public SignUpController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // ------------------- SIGN UP -------------------
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(NewUser model)
        {
            if (ModelState.IsValid)
            {
                using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DBConnection")))
                {
                    con.Open();

                    // Check if email already exists
                    SqlCommand checkCmd = new SqlCommand("SELECT COUNT(*) FROM tb_usr_master WHERE email = @Email", con);
                    checkCmd.Parameters.AddWithValue("@Email", model.email);
                    int userExists = (int)checkCmd.ExecuteScalar();

                    if (userExists > 0)
                    {
                        ViewBag.Message = "Email already registered.";
                        return View(model);
                    }

                    // Insert new user
                    SqlCommand cmd = new SqlCommand("InsertUser", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Add parameters for the stored procedure
                    cmd.Parameters.AddWithValue("@usr_name", model.user_name);
                    cmd.Parameters.AddWithValue("@email", model.email);
                    cmd.Parameters.AddWithValue("@usr_pwd", model.password);
                    cmd.Parameters.AddWithValue("@usr_type", model.UserType);

                    // Only pass donor fields if applicable
                    if (model.UserType == "Donor")
                    {
                        cmd.Parameters.AddWithValue("@BloodGroup", model.bloodGroup ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@UnitsAvailable", model.unitsAvailable ?? (object)DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@BloodGroup", DBNull.Value);
                        cmd.Parameters.AddWithValue("@UnitsAvailable", DBNull.Value);
                    }

                    // Execute the stored procedure
                    cmd.ExecuteNonQuery();

                    TempData["SuccessMessage"] = "Registration successful! Please login.";
                    return RedirectToAction("Login", "Login");

                }
            }

            return View(model);
        }

        // ------------------- LOGIN -------------------
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM UserMaster WHERE email = @Email AND password = @Password", con);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Password", password);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    var model = new UserModel
                    {
                        usr_id = Convert.ToInt32(reader["user_id"]),
                        patient_name = reader["patient_name"].ToString(),
                        email = reader["email"].ToString(),
                        //blood_grp_id = Convert.ToInt32(["blood_grp_id"])
                    };

                    HttpContext.Session.SetString("UserEmail", model.email);
                    HttpContext.Session.SetString("UserName", model.patient_name);

                    return RedirectToAction("Dashboard", "User", model);
                }
                else
                {
                    ViewBag.Error = "Invalid email or password.";
                    return View();
                }
            }
        }

        // ------------------- LOGOUT -------------------
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
