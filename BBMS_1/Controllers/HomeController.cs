using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BBMS_1.Models;

namespace BBMS_1.Controllers;

public class HomeController : Controller
{

    public ActionResult Index()
    {
        return View();
    }
    public IActionResult Dashboard()
        {
            // Retrieve user details from session
            var user = HttpContext.Session.GetString("Username");
            var email = HttpContext.Session.GetString("Email");
           // var role = HttpContext.Session.GetString("Role");

            if (string.IsNullOrEmpty(user))
            {
                return RedirectToAction("Login", "Login"); // Redirect if not logged in
            }

            var model = new UserModel
            {
                patient_name = user,
                email = email,
                //Role = role
            };

            return View(model);
        }
}


