using Microsoft.AspNetCore.Identity;

namespace ApplicationService.DAL.Contexts
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string PhoneNumber { get; set; }

        public string DoB { get; set; }
    }
}
