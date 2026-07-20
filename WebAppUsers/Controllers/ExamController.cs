using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> TakeExam(int? subjectId)
        {
            string currentUsername = User.Identity.Name;
            var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == currentUsername);

            if (currentUser != null && currentUser.Role == "Student" && subjectId.HasValue)
            {
                var takenSubjects = (currentUser.TakenSubjectIds ?? "")
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => s.Trim())
                    .ToList();

                if (takenSubjects.Contains(subjectId.Value.ToString()))
                {
                    ViewBag.Message = "Sorry, you've already taken this subject's exam and you can't take it again!";
                    return View("ExamAlreadyTaken");
                }
            }

            if (subjectId == null || subjectId == 0)
            {
                var subjects = await _context.Subjects.ToListAsync();
                ViewBag.Subjects = subjects;
                return View(new List<Question>());
            }

            var questions = await _context.Questions
                .Include(q => q.Subject)
                .Where(q => q.SubjectId == subjectId.Value)
                .ToListAsync();

            var selectedSubject = await _context.Subjects.FindAsync(subjectId.Value);

            ViewBag.SelectedSubjectName = selectedSubject?.Name;
            ViewBag.SelectedSubjectId = subjectId.Value;
            ViewBag.Subjects = await _context.Subjects.ToListAsync();

            return View(questions);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> SubmitExam(Dictionary<int, string> answers, int subjectId)
        {
            int score = 0;

            int totalQuestions = await _context.Questions.CountAsync(q => q.SubjectId == subjectId);

            if (answers != null)
            {
                foreach (var item in answers)
                {
                    int questionId = item.Key;
                    string studentAnswer = item.Value;

                    var question = await _context.Questions.FirstOrDefaultAsync(q => q.Id == questionId);
                    if (question != null && question.CorrectAnswer == studentAnswer)
                    {
                        score++;
                    }
                }
            }

            string currentUsername = User.Identity.Name;
            var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == currentUsername);

            if (currentUser != null && currentUser.Role == "Student")
            {
                var takenSubjects = (currentUser.TakenSubjectIds ?? "")
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => s.Trim())
                    .ToList();

                if (!takenSubjects.Contains(subjectId.ToString()))
                {
                    takenSubjects.Add(subjectId.ToString());
                    currentUser.TakenSubjectIds = string.Join(",", takenSubjects);
                }

               
                var scoresDictionary = new Dictionary<string, string>();

                if (!string.IsNullOrEmpty(currentUser.SubjectScores))
                {
                    var scoreEntries = currentUser.SubjectScores.Split(',', StringSplitOptions.RemoveEmptyEntries);
                    foreach (var entry in scoreEntries)
                    {
                        var parts = entry.Split(':');
                        if (parts.Length == 2 && !scoresDictionary.ContainsKey(parts[0]))
                        {
                            scoresDictionary.Add(parts[0].Trim(), parts[1].Trim());
                        }
                    }
                }

                scoresDictionary[subjectId.ToString()] = score.ToString();

                currentUser.SubjectScores = string.Join(",", scoresDictionary.Select(x => $"{x.Key}:{x.Value}"));

                currentUser.ExamScore = score;

                await _context.SaveChangesAsync();
            }

            ViewBag.ScoreResult = $"You scored {score} out of {totalQuestions}!";
            return View("ExamResult");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            var model = new CreateQuestionViewModel
            {
                Subjects = _context.Subjects.Select(s => new SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = s.Name
                }).ToList()
            };

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(CreateQuestionViewModel model)
        {
            if (ModelState.IsValid)
            {
                Question newQuestion = new Question
                {
                    QuestionText = model.QuestionText,
                    OptionA = model.OptionA,
                    OptionB = model.OptionB,
                    OptionC = model.OptionC,
                    CorrectAnswer = model.CorrectAnswer,
                    SubjectId = model.SubjectId
                };

                _context.Questions.Add(newQuestion);
                await _context.SaveChangesAsync();

                return RedirectToAction("TakeExam", new { subjectId = model.SubjectId });
            }

            model.Subjects = _context.Subjects.Select(s => new SelectListItem
            {
                Value = s.Id.ToString(),
                Text = s.Name
            }).ToList();

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("Exam/Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var question = await _context.Questions.FirstOrDefaultAsync(q => q.Id == id);
            int? redirectedSubjectId = null;

            if (question != null)
            {
                redirectedSubjectId = question.SubjectId;
                _context.Questions.Remove(question);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("TakeExam", new { subjectId = redirectedSubjectId });
        }
    }
}