using ApplicationService.DAL.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace ApplicationService.DAL.Contexts
{
    public class AppDbContext: IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
        }
        public DbSet<Statement> Statements { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<GlobalExam> GlobalExams { get; set; }
        public DbSet<ExamImage> ExamImages { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<AvgGrade> AvgGrades { get; set; }




    }
}
