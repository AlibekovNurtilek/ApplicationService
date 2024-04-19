using System.ComponentModel.DataAnnotations;

namespace ApplicationService.BLL.Models.AuthModels
{
    public class RegisterModel
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]

        public string Email { get; set; }
        [Required]
        public string PhoneNumber { get; set; }

        public string DoB { get; set; }

        public IFormFile PassportFront { get; set; }
        public IFormFile PassportBack { get; set; }
        public IFormFile DiplomImage { get; set; }
    }
}
