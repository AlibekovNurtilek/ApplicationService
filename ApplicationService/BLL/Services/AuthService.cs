using ApplicationService.BLL.Models.AuthModels;
using ApplicationService.BLL.Models.Responces;
using ApplicationService.DAL.Contexts;
using ApplicationService.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace ApplicationService.BLL.Services
{
    public interface IAuthService
    {
        public Task<AuthResponse> Register(RegisterModel registerModel);
    }
    public class AuthService:IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppDbContext _context;
        public AuthService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, AppDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        public async Task<AuthResponse> Register(RegisterModel registerModel)
        {
            var isExistsUser = await _userManager.FindByEmailAsync(registerModel.Email);
            if (isExistsUser is not null)
                return new AuthResponse { Message = "This Email already exists" };
            var newStatement = new Statement
            {
                FirstName = registerModel.FirstName,
                LastName = registerModel.LastName,
                Email = registerModel.Email,
                PhoneNumber = registerModel.PhoneNumber,
                DoB = registerModel.DoB,
                IsMagistr = registerModel.IsMagistr,

            };

            return null;
            
        }
    }
}
