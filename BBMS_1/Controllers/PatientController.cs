using BBMS_1.Data;
using BBMS_1.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace BBMS_1.Controllers
{
    public class PatientController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PatientController(ApplicationDbContext context)
        {
            _context = context;
        }

        //public IActionResult PatientHome(int userId)
        //{
            
        //}
    }
}
