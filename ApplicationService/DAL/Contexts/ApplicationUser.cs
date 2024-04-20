using ApplicationService.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApplicationService.DAL.Contexts
{
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? MiddleName { get; set; }
        public string? PhoneNumber { get; set; }

        public string? DoB { get; set; }
        [ForeignKey("DepartmentId")]
        public int DepartmentId { get; set; }
        public Department Department { get; set; }

        public string? ImagePath;
        public string? PassportFrontImage { get; set; }
        public string? PassportBackImage { get; set; }
        public string? DiplomImage { get; set; }
        public ICollection<GlobalExamStudent> GlobalExamStudents { get; set; }

    }
}
