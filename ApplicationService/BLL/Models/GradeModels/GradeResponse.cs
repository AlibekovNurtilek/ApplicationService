using ApplicationService.DAL.Contexts;
using ApplicationService.DAL.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApplicationService.BLL.Models.GradeModels
{
    public class GradeResponse
    {
        public int Id { get; set; }

        public virtual Exam Exam { get; set; }
        public virtual ApplicationUser ApplicationUserStud { get; set; }
        public virtual ApplicationUser ApplicationUserEmp { get; set; }
        public double StudGrade { get; set; }
    }
}
