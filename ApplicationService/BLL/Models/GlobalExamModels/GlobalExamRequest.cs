namespace ApplicationService.BLL.Models.GlobalExamModels
{
    public class GlobalExamRequest
    {
        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string ClassRoom { get; set; }
    }
}
