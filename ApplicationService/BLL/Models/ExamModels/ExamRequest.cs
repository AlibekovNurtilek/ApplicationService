using ApplicationService.DAL.Contexts;
using ApplicationService.DAL.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApplicationService.BLL.Models.ExamModels
{
    public class ExamRequest
    {
        public int GlobalExamId { get; set; }

        public string ApplicationUserStudId { get; set; }
        public List<IFormFile> ExamImages { get; set; }
    }
}
