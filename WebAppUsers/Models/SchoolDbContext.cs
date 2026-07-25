using Microsoft.EntityFrameworkCore;
using WebAppUsers.Models;

namespace WebAppUsers.Context
{
    public class SchoolDbContext : DbContext
    {
        public SchoolDbContext(DbContextOptions<SchoolDbContext> options) : base(options)
        {
        }

        public DbSet<Question> Questions { get; set; }
        public DbSet<User> Users { get; set; }

        public DbSet<Subject> Subjects { get; set; }
    }
}