using System.ComponentModel.DataAnnotations;

namespace ApplicationService.BLL.Models
{
    public class StatementsModel
    {
        public string FirstName { get; set; }
        
        public string LastName { get; set; }

        public string Email { get; set; }
       
        public string PhoneNumber { get; set; }

        public string DoB { get; set; }

        public bool IsMagistr { get; set; }

       
        public int DepartmentId { get; set; }

        public string PassportFront { get; set; }
        public string PassportBack { get; set; }
        public string DiplomImage { get; set; }
    }
}
