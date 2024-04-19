using ApplicationService.DAL.Contexts;
using ApplicationService.DAL.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApplicationService.BLL.Models.ExamModels
{
    public class ExamResponse
    {
        public int Id { get; set; }
        public virtual ApplicationUser ApplicationUserStud { get; set; }
        public virtual ApplicationUser ApplicationUserEmp { get; set; }
        public virtual GlobalExam GlobalExam { get; set; }

        public ICollection<ExamImage> ExamImages { get; set; }
    }
}
