using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebAppUsers.Models
{
    public class CreateQuestionViewModel
    {
        [Required(ErrorMessage = "Question text is required.")]
        [Display(Name = "السؤال")]
        public string QuestionText { get; set; }

        [Required(ErrorMessage = "Option A is required.")]
        [Display(Name = "الاختيار A")]
        public string OptionA { get; set; }

        [Required(ErrorMessage = "Option B is required.")]
        [Display(Name = "الاختيار B")]
        public string OptionB { get; set; }

        [Required(ErrorMessage = "Option C is required.")]
        [Display(Name = "الاختيار C")]
        public string OptionC { get; set; }

        [Required(ErrorMessage = "You must specify the correct answer.")]
        [Display(Name = "الإجابة الصحيحة")]
        public string CorrectAnswer { get; set; }

        [Required(ErrorMessage = "Please select a subject.")]
        [Display(Name = "المادة")]
        public int SubjectId { get; set; }

        public List<SelectListItem>? Subjects { get; set; }
    }
}