namespace WebAppUsers.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }

        public string TakenSubjectIds { get; set; } = ""; 

        public string SubjectScores { get; set; } = ""; 

        public int ExamScore { get; set; } = 0; 
    }
}