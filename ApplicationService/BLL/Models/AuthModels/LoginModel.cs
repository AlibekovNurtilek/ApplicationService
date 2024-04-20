using System.ComponentModel.DataAnnotations;

namespace ApplicationService.BLL.Models.AuthModels
{
    public class LoginModel
    {
        [Required(ErrorMessage = "UserName is required")]
        [EmailAddress]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }

    }
}
