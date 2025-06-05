using Microsoft.AspNetCore.Mvc;
using BBMS_1.Models;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace BBMS_1.Controllers
{
    public class BloodDonationController : Controller
    {
        private readonly IConfiguration _configuration;

        public BloodDonationController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(BloodDonationModel model)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Index", "Dashboard", new { userId = model.UserId });

            string connStr = _configuration.GetConnectionString("DBConnection");

            using (SqlConnection conn = new SqlConnection(connStr))
            using (SqlCommand cmd = new SqlCommand("InsertBloodDonation", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserId", model.UserId);
                cmd.Parameters.AddWithValue("@BloodGroup", model.BloodGroup);
                cmd.Parameters.AddWithValue("@UnitsAvailable", model.Units);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            TempData["SuccessMessage"] = "Blood Donation recorded successfully!";
            return RedirectToAction("Index", "Dashboard", new { userId = model.UserId });
        }
    }
}
