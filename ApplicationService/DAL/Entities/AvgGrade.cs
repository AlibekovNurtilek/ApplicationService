using ApplicationService.DAL.Contexts;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApplicationService.DAL.Entities
{
    public class AvgGrade
    {
        public int Id { get; set; }
        [ForeignKey("ApplicationUserId")]
        public int ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
