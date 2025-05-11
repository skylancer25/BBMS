using Microsoft.AspNetCore.Mvc;
using BBMS_1.Models;
using BBMS_1.Data; // Add this for database access (_context)
using Microsoft.AspNetCore.Http; // For session usage
using System.Linq; // For .Where(), .ToList()
using System.Threading.Tasks; // For async methods
using Microsoft.EntityFrameworkCore; // Required for ToListAsync()
using Microsoft.Data.SqlClient;
using System.Data;

namespace BBMS_1.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        // This action will show the User Profile page
        public IActionResult UserProfile()
        {
            // Try to fetch user details from session
            var patientName = HttpContext.Session.GetString("patient_name");
            var bloodGroup = HttpContext.Session.GetString("blood_grp");
            var email = HttpContext.Session.GetString("email");
            var address = HttpContext.Session.GetString("address");
            var pincode = HttpContext.Session.GetString("pincode");

            // If session values are missing, you can redirect to Login
            if (string.IsNullOrEmpty(patientName))
            {
                return RedirectToAction("Login", "Login"); // Assuming you have LoginController
            }

            // Fetch the user's past blood requests
            var userRequests = _context.BloodRequests
                                .Where(r => r.PatientName == patientName)
                                .OrderByDescending(r => r.RequestDate)
                                .ToList(); // Use ToList() if not async

            // Prepare ViewModel with both profile and requests
            var model = new UserProfileViewModel
            {
                patient_name = patientName,
                blood_grp = bloodGroup,
                email = email,
                Address = address,
                Pincode = pincode,
                BloodRequests = userRequests
            };

            return View(model);
        }

        // AJAX method to get blood requests of the user
        [HttpGet]
        public async Task<IActionResult> GetBloodRequests()
        {
            var patientName = HttpContext.Session.GetString("patient_name");
            if (string.IsNullOrEmpty(patientName))
            {
                return Content("<p class='text-muted text-center'>No blood requests made yet.</p>");
            }

            var bloodRequests = await _context.BloodRequests
                                                .Where(br => br.PatientName == patientName)
                                                .OrderByDescending(br => br.RequestDate)
                                                .ToListAsync(); // Use ToListAsync here for async fetch

            // Build HTML table rows manually
            if (!bloodRequests.Any())
            {
                return Content("<p class='text-muted text-center'>No blood requests made yet.</p>");
            }

            var html = @"<div class='table-responsive'><table class='table table-bordered table-hover mt-3'>
                            <thead class='table-dark'>
                                <tr>
                                    <th>#</th>
                                    <th>Blood Group</th>
                                    <th>Additional Details</th>
                                    <th>Request Date</th>
                                    <th>Status</th>
                                </tr>
                            </thead><tbody>";

            foreach (var request in bloodRequests)
            {
                html += $@"<tr>
                            <td>{request.Id}</td>
                            <td>{request.RequiredBloodGroup}</td>
                            <td>{request.AdditionalDetails}</td>
                            <td>{request.RequestDate:dd MMM yyyy}</td>
                            <td>";

                if (request.Status == "Pending")
                    html += "<span class='badge bg-warning text-dark'>Pending</span>";
                else if (request.Status == "Approved")
                    html += "<span class='badge bg-primary'>Approved</span>";
                else if (request.Status == "Fulfilled")
                    html += "<span class='badge bg-success'>Fulfilled</span>";
                else
                    html += "<span class='badge bg-secondary'>Unknown</span>";

                html += "</td></tr>";
            }

            html += "</tbody></table></div>";

            return Content(html, "text/html");
        }

        // Action to submit a new blood request
        [HttpPost]
        public async Task<IActionResult> SubmitBloodRequest(BloodRequestModel bloodRequest)
        {
            var patientName = HttpContext.Session.GetString("patient_name");

            if (string.IsNullOrEmpty(patientName))
            {
                return RedirectToAction("Login", "Login"); // Redirect to login if session is invalid
            }

            // Set additional properties
            bloodRequest.PatientName = patientName;
            bloodRequest.RequestDate = DateTime.Now;
            bloodRequest.Status = "Pending"; // Default status

            _context.BloodRequests.Add(bloodRequest);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Blood request submitted successfully!";

            // Return the updated blood requests history table
            return RedirectToAction("UserProfile");
        }

        [HttpGet]
        public IActionResult CheckBloodAvailability(int userId)
        {
            int isAvailable = 0;

            using (SqlConnection conn = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("CheckBloodAvailability", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserId", userId);

                    conn.Open();
                    isAvailable = (int)cmd.ExecuteScalar();
                }
            }

            return Json(new { available = isAvailable == 1 });
        }

    }
}
