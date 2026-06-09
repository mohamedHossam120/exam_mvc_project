using System.ComponentModel.DataAnnotations;

namespace WebAppUsers.Models
{
    public class CreateStudentViewModel
    {
        [Required(ErrorMessage = "Student name is required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 50 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Age is required")]
        public string Age { get; set; }
    }
}