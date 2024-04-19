﻿using ApplicationService.DAL.Contexts;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApplicationService.DAL.Entities
{
    public class GlobalExam
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string ClassRoom { get; set; }
        [ForeignKey("DepartmentId")]

        public int DepartmentId { get; set; }
        public Department Department { get; set; }
        ICollection<Exam> Exam { get; set; }
        ICollection<ApplicationUser> User { get; set; }
        
    }
}
