using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAppUsers.Context;
using WebAppUsers.Models;
using System.Linq;

namespace WebAppUsers.Controllers
{
    [Authorize(Roles = "Admin")] // متاح فقط للمسؤولين
    public class SubjectController : Controller
    {
        private readonly SchoolDbContext _context;

        public SubjectController(SchoolDbContext context)
        {
            _context = context;
        }

        // GET: /Subject/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Subject/Create
        [HttpPost]
        public IActionResult Create(Subject subject)
        {
            if (ModelState.IsValid)
            {
                // التحقق من عدم تكرار اسم المادة
                var exists = _context.Subjects.Any(s => s.Name.ToLower() == subject.Name.ToLower());
                if (exists)
                {
                    ModelState.AddModelError("Name", "هذه المادة مضافة بالفعل!");
                    return View(subject);
                }

                _context.Subjects.Add(subject);
                _context.SaveChanges();

                return RedirectToAction("Create", "Exam");
            }

            return View(subject);
        }

        [HttpGet]
        public IActionResult Index()
        {
            var subjects = _context.Subjects.ToList();
            return View(subjects);
        }
    }
}