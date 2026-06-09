using System.ComponentModel.DataAnnotations;

namespace WebAppUsers.Models
{
    public class CreateQuestionViewModel
    {
        [Required(ErrorMessage = "Question text is required.")]
        public string QuestionText { get; set; }

        [Required(ErrorMessage = "Option A is required.")]
        public string OptionA { get; set; }

        [Required(ErrorMessage = "Option B is required.")]
        public string OptionB { get; set; }

        [Required(ErrorMessage = "Option C is required.")]
        public string OptionC { get; set; }

        [Required(ErrorMessage = "You must specify the correct answer.")]
        public string CorrectAnswer { get; set; }
    }
}