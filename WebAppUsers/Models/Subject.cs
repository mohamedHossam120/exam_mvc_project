using System.ComponentModel.DataAnnotations;

namespace WebAppUsers.Models
{
    public class Subject
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "The subject name is required")]
        [Display(Name = "Subject name")]
        public string Name { get; set; }

        [Display(Name = "Subject code")]
        public string? Code { get; set; }

        public ICollection<Question> Questions { get; set; } = new List<Question>();
    }
}