using ApplicationService.BLL.Models.AuthModels;
using ApplicationService.BLL.Models.Responces;
using ApplicationService.DAL.Contexts;
using ApplicationService.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics.Contracts;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApplicationService.BLL.Services
{
    public interface IAuthService
    {
        public Task<AuthResponse> RegisterAsync(RegisterModel model);
        public Task<AuthResponse> LoginAsync(LoginModel model);
    }
    public class AuthService : IAuthService
    {
       
        private readonly AppDbContext _context;
        private readonly IAmazonService _amazonService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AuthService(AppDbContext context, IAmazonService amazonService, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _context = context;
            _amazonService = amazonService;
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        public async Task<AuthResponse> RegisterAsync(RegisterModel model)
        {
            try
            {
                var isUserExists = await _context.Statements.Where(u => u.Email == model.Email).FirstOrDefaultAsync();
                if (isUserExists is not null)
                    return new AuthResponse { Message = "This email already exist" };
                var statement = new Statement
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Phone = model.PhoneNumber,
                    DoB = model.DoB,
                    IsMagistr = model.IsMagistr,
                    DepartmentId = model.DepartmentId,
                };
                var passportFrontBase64 = await ConvertToBase64(model.PassportFront);
                var passportBackBase64 = await ConvertToBase64(model.PassportBack);
                var diplomImageBase64 = await ConvertToBase64(model.DiplomImage);

                statement.PassportFrontImage = await _amazonService.UploadImage(passportFrontBase64);
                statement.PassportBackImage = await _amazonService.UploadImage(passportBackBase64);
                statement.DiplomImage = await _amazonService.UploadImage(diplomImageBase64);

                await _context.AddAsync(statement);
                await _context.SaveChangesAsync();

                return new AuthResponse { Success = true, Message = "Succes" };
            }
            catch(Exception ex)
            {
                return new AuthResponse { Success = false, Message = ex.Message };
            }

            

        }
        private async Task<string> ConvertToBase64(IFormFile photo)
        {
            if (photo == null || photo.Length == 0)
            {
                throw new ArgumentException("Фото не может быть пустым");
            }

            using (var ms = new MemoryStream())
            {
                await photo.CopyToAsync(ms);
                byte[] photoBytes = ms.ToArray();
                string base64String = Convert.ToBase64String(photoBytes);
                return $"data:{photo.ContentType};base64,{base64String}";
            }
        }

        public async Task<AuthResponse> LoginAsync(LoginModel model)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, model.Password);
                if (!isPasswordCorrect)
                    return new AuthResponse()
                    {
                        Success = false,
                        Message = "Invalid Credentials"
                    };
                var tokenObject = await GenerateNewJsonWebToken(user);
                var JwtToken = new JwtSecurityTokenHandler().WriteToken(tokenObject);

                return new AuthResponse()
                {
                    Success = true,
                    AccessToken = JwtToken,
                };
            }
            catch (Exception ex)
            {
                return new AuthResponse()
                {
                    Message = ex.Message,
                };
            }
           
        }
        private async Task<JwtSecurityToken> GenerateNewJsonWebToken(ApplicationUser user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString())

            };
            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole));
            }
            var authSecret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            string expiryTimeFrameValue = _configuration["JWT:ExpiryTimeFrame"];

            TimeSpan expiryTimeFrame = TimeSpan.Parse(expiryTimeFrameValue);


            var tokenObject = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.UtcNow.Add(expiryTimeFrame),
                    claims: claims,
                    signingCredentials: new SigningCredentials(authSecret, SecurityAlgorithms.HmacSha256)
                );

            return tokenObject;
        }

    }
}
