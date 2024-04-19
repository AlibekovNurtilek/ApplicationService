using ApplicationService.DAL.Contexts;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApplicationService.DAL.Entities
{
    public class Grade
    {
        public int Id { get; set; }
        [ForeignKey("ExamId")]
        public int ExamId { get; set; }
        public virtual Exam Exam { get; set; }
        [ForeignKey("ApplicationUserStudId")]
        public string ApplicationUserStudId { get; set; }
        public virtual ApplicationUser ApplicationUserStud { get; set; }
        [ForeignKey("ApplicationUserEmpId")]
        public string ApplicationUserEmpId { get; set; }
        public virtual ApplicationUser ApplicationUserEmp { get; set; }
        public double StudGrade { get; set; }
    }
}
