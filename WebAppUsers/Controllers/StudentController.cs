using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using WebAppUsers.Context;

namespace WebAppUsers.Controllers
{
    [Authorize(Roles = "Admin")] 
    public class StudentController : Controller
    {
        private readonly SchoolDbContext _context;

        public StudentController(SchoolDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var students = _context.Users
                                  .Where(u => u.Role == "Student")
                                  .ToList();

            return View(students);
        }
    }
}
