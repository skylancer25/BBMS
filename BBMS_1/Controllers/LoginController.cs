using BBMS_1.Data;
using BBMS_1.Models;
using Microsoft.AspNetCore.Mvc;

namespace BBMS_1.Controllers
{
    public class LoginController : Controller
    {
        // GET: Account/Login

        private readonly IUserInterface _userRepo;


        public LoginController(IUserInterface userRepo)
        {
            _userRepo = userRepo;

        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginModel model)
        {
            
                int objresult = _userRepo.ValidateUser(model.UserName, model.Password);
            bool isAuthenticated = false;
            if (objresult > 0)
            {
                isAuthenticated = true;
            }
                if (isAuthenticated)
                {
                HttpContext.Session.SetString("Username", model.UserName);

                // ✅ Redirect to Home Page on successful login
                // return RedirectToAction("Dashboard", "Home");
                return RedirectToAction("Index", "Dashboard", new { userId = objresult });

            }

            // ❌ Show error message for invalid login
            ViewBag.ErrorMessage = "Invalid username or password";
                return View();

            }



        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); // Remove session data
            return RedirectToAction("Login", "Login"); // Redirect to login page
        }

    }
}


