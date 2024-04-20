using System.ComponentModel.DataAnnotations.Schema;

namespace ApplicationService.DAL.Entities
{
    public class Statement
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string DoB { get; set; }

        public bool IsMagistr {  get; set; }    

        public bool IsAccespted { get; set; }
        public string PassportFrontImage { get; set; }
        public string PassportBackImage { get;set; }
        public string DiplomImage { get; set; }

        [ForeignKey("DepartmentId")]
        public int DepartmentId { get; set; }
        public Department Department { get; set; }  
    }
}
