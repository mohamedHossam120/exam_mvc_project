using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebAppUsers.Context;
using WebAppUsers.Models;

namespace WebAppUsers.Controllers
{
    public class AccountController : Controller
    {
        private readonly SchoolDbContext _context;

        public AccountController(SchoolDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model, string role)
        {
            if (ModelState.IsValid)
            {
                var userExists = _context.Users.Any(u => u.Username == model.Username || u.Email == model.Email);
                if (userExists)
                {
                    ModelState.AddModelError("", "Username or Email already exists.");
                    return View(model);
                }

                string finalRole = "Student";

                if (role == "Admin")
                {
                    if (User.Identity.IsAuthenticated && User.IsInRole("Admin"))
                    {
                        finalRole = "Admin";
                    }
                    else
                    {
                        finalRole = "Student";
                    }
                }

                User newUser = new User
                {
                    Username = model.Username,
                    Email = model.Email,
                    Password = model.Password,
                    Role = finalRole,
                    TakenSubjectIds = "", 
                    ExamScore = 0
                };

                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();

                if (User.Identity.IsAuthenticated && User.IsInRole("Admin"))
                {
                    return RedirectToAction("Index", "Student");
                }

                return RedirectToAction("Login");
            }
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")] 
        public IActionResult CreateAdmin()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateAdmin(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userExists = _context.Users.Any(u => u.Username == model.Username || u.Email == model.Email);
                if (userExists)
                {
                    ModelState.AddModelError("", "Username or Email already exists.");
                    return View(model);
                }

                User newAdmin = new User
                {
                    Username = model.Username,
                    Email = model.Email,
                    Password = model.Password,
                    Role = "Admin",
                    TakenSubjectIds = "", 
                    ExamScore = 0
                };

                _context.Users.Add(newAdmin);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Student");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Users.FirstOrDefault(u => u.Username == model.Username && u.Password == model.Password);

                if (user != null)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.Username),
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim(ClaimTypes.Role, user.Role)
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, "CookieAuth");

                    await HttpContext.SignInAsync("CookieAuth", new ClaimsPrincipal(claimsIdentity));

                    if (user.Role == "Admin")
                    {
                        return RedirectToAction("Index", "Student");
                    }

                    return RedirectToAction("TakeExam", "Exam");
                }

                ModelState.AddModelError("", "Invalid username or password.");
            }
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("CookieAuth");
            return RedirectToAction("Login");
        }
    }
}