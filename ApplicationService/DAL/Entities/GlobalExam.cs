using ApplicationService.DAL.Contexts;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApplicationService.DAL.Entities
{
    public class GlobalExam
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string ClassRoom { get; set; }
        [ForeignKey("DepartmentId")]

        public int DepartmentId { get; set; }
        public Department Department { get; set; }

        [ForeignKey("ExamId")]
        public int ExamId { get; set; }
        public ICollection<Exam> Exam { get; set; }
        public ICollection<GlobalExamStudent> GlobalExamStudents { get; set; }
        
    }
}
