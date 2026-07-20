using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAppUsers.Models
{
    public class Question
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "The text of the question is required.")]
        [Display(Name = "The question")]
        public string QuestionText { get; set; }

        [Required(ErrorMessage = "Option A is required.")]
        [Display(Name = "Option A")]
        public string OptionA { get; set; }

        [Required(ErrorMessage = "Option B is required.")]
        [Display(Name = "Option B")]
        public string OptionB { get; set; }

        [Required(ErrorMessage = "Option C is required.")]
        [Display(Name = "Option C")]
        public string OptionC { get; set; }

        [Required(ErrorMessage = "The correct answer is needed.")]
        [Display(Name = "The right answer")]
        public string CorrectAnswer { get; set; }

        [Required(ErrorMessage = "You have to choose the subject.")]
        [Display(Name = "Subject")]
        public int SubjectId { get; set; }

        [ForeignKey("SubjectId")]
        public Subject? Subject { get; set; }
    }
}