using ApplicationService.DAL.Contexts;
using ApplicationService.DAL.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApplicationService.BLL.Models.GradeModels
{
    public class GradeRequest
    {
        public int ExamId { get; set; }
        public string ApplicationUserStudId { get; set; }
        public double StudGrade { get; set; }
    }
}
