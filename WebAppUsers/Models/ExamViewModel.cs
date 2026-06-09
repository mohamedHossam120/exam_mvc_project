using System.ComponentModel.DataAnnotations;

namespace WebAppUsers.Models
{
    public class ExamViewModel
    {
        public int QuestionId { get; set; }
        public string QuestionText { get; set; }
        public string OptionA { get; set; }
        public string OptionB { get; set; }
        public string OptionC { get; set; }

        [Required(ErrorMessage = "Please select an answer before submitting!")]
        public string SelectedAnswer { get; set; }
    }
}