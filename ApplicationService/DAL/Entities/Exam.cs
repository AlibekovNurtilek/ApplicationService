﻿using ApplicationService.DAL.Contexts;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApplicationService.DAL.Entities
{
    public class Exam
    {
        public int Id { get; set; }
        [ForeignKey("GlobalExamId")]
        public int GlobalExamId { get; set; }
        public virtual GlobalExam GlobalExam { get; set; }
        [ForeignKey("ApplicationUserStudId")]
        public string ApplicationUserStudId { get; set; }
        public virtual ApplicationUser ApplicationUserStud { get; set; }
        [ForeignKey("ApplicationUserEmpId")]
        public string ApplicationUserEmpId { get; set; }
        public virtual ApplicationUser ApplicationUserEmp { get; set; }
        public ICollection<ExamImage> ExamImages { get; set; }

    }
}
