using System.ComponentModel.DataAnnotations.Schema;

namespace ApplicationService.DAL.Entities
{
    public class ExamImage
    {
        public int Id { get; set; }
        public string url { get; set; }
        [ForeignKey("ExamId")]
        public int ExamId { get; set; }
        public virtual Exam Exam { get; set; }
    }
}
