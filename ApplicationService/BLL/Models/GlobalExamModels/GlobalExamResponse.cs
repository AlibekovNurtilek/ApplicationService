using ApplicationService.DAL.Contexts;
using ApplicationService.DAL.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApplicationService.BLL.Models.GlobalExamModels
{
    public class GlobalExamResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string ClassRoom { get; set; }
        public Department Department { get; set; }
        ICollection<Exam> Exam { get; set; }
        ICollection<ApplicationUser> User { get; set; }
    }
}
