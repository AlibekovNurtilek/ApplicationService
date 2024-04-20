using ApplicationService.DAL.Contexts;

namespace ApplicationService.DAL.Entities
{
    public class GlobalExamStudent
    {
        public int GlobalExamId { get; set; }
        public GlobalExam GlobalExam { get; set; }

        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
