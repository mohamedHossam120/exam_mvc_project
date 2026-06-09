using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using WebAppUsers.Context;
using WebAppUsers.Models;

namespace WebAppUsers.Controllers
{
    public class ExamController : Controller
    {
        private readonly SchoolDbContext _context;

        public ExamController(SchoolDbContext context)
        {
            _context = context;
        }

        // GET: /Exam/TakeExam
        [Authorize]
        [HttpGet]
        public IActionResult TakeExam()
        {
            string currentUsername = User.Identity.Name;
            var currentUser = _context.Users.FirstOrDefault(u => u.Username == currentUsername);

            // Prevent students from retaking the exam if they already submitted before
            if (currentUser != null && currentUser.Role == "Student" && currentUser.HasTakenExam)
            {
                ViewBag.Message = "Sorry, you have already taken this exam. You cannot retake it!";
                return View("ExamAlreadyTaken");
            }

            var allQuestions = _context.Questions.ToList();
            return View(allQuestions);
        }

        // POST: /Exam/SubmitExam
        [HttpPost]
        public IActionResult SubmitExam(Dictionary<int, string> answers)
        {
            int score = 0;
            int totalQuestions = _context.Questions.Count();

            // Calculate the actual exam score
            foreach (var item in answers)
            {
                int questionId = item.Key;
                string studentAnswer = item.Value;

                var question = _context.Questions.FirstOrDefault(q => q.Id == questionId);
                if (question != null && question.CorrectAnswer == studentAnswer)
                {
                    score++;
                }
            }

            string currentUsername = User.Identity.Name;
            var currentUser = _context.Users.FirstOrDefault(u => u.Username == currentUsername);

            // Update student record and save the final score to the database
            if (currentUser != null && currentUser.Role == "Student")
            {
                currentUser.HasTakenExam = true;
                currentUser.ExamScore = score; // 🛑 Saves the dynamic score persistently to SQL Server
                _context.SaveChanges();
            }

            ViewBag.ScoreResult = $"You scored {score} out of {totalQuestions}!";
            return View("ExamResult");
        }

        // GET: /Exam/Create
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Exam/Create
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Create(CreateQuestionViewModel model)
        {
            if (ModelState.IsValid)
            {
                Question newQuestion = new Question
                {
                    QuestionText = model.QuestionText,
                    OptionA = model.OptionA,
                    OptionB = model.OptionB,
                    OptionC = model.OptionC,
                    CorrectAnswer = model.CorrectAnswer
                };

                _context.Questions.Add(newQuestion);
                _context.SaveChanges();

                return RedirectToAction("TakeExam");
            }

            return View(model);
        }

        // POST: /Exam/Delete/{id}
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("Exam/Delete/{id}")]
        public IActionResult Delete(int id)
        {
            var question = _context.Questions.FirstOrDefault(q => q.Id == id);

            if (question != null)
            {
                _context.Questions.Remove(question);
                _context.SaveChanges();
            }

            return RedirectToAction("TakeExam");
        }
    }
}