namespace ApplicationService.DAL.Entities
{
    public class GlobalExam
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string ClassRoom { get; set; }

    }
}
