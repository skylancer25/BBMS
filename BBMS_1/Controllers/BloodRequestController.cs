using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data;
using Microsoft.Data.SqlClient;
using BBMS_1.Models;
using Azure.Core;

public class BloodRequestController : Controller
{
    private readonly IConfiguration _configuration;

    public BloodRequestController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(BloodRequestModel model, [FromQuery] int userId)
    {
        int requestNo = 0;
        string connectionString = _configuration.GetConnectionString("DBConnection");

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            using (SqlCommand cmd = new SqlCommand("InsertBloodRequest", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@RequiredBloodGroup", model.RequiredBloodGroup);
                cmd.Parameters.AddWithValue("@AdditionalDetails", model.AdditionalDetails ?? "");
                cmd.Parameters.AddWithValue("@UnitsRequired", model.UnitsRequired);
                cmd.Parameters.AddWithValue("@PatientId", userId);
                cmd.Parameters.Add("@RequestNo", SqlDbType.Int).Direction = ParameterDirection.Output;

                conn.Open();
                cmd.ExecuteNonQuery();

                requestNo = Convert.ToInt32(cmd.Parameters["@RequestNo"].Value);
            }
        }

        TempData["SuccessMessage"] = $"Request submitted successfully. Request No: {requestNo}";
        return RedirectToAction("Index", "Dashboard", new { userId = userId });
    }

    
}
