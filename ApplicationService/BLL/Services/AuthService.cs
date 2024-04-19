using ApplicationService.BLL.Models.AuthModels;
using ApplicationService.BLL.Models.Responces;
using ApplicationService.DAL.Contexts;
using ApplicationService.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics.Contracts;

namespace ApplicationService.BLL.Services
{
    public interface IAuthService
    {
        public Task<AuthResponse> RegisterAsync(RegisterModel model);
    }
    public class AuthService : IAuthService
    {
       
        private readonly AppDbContext _context;
        private readonly IAmazonService _amazonService;
        public AuthService(AppDbContext context, IAmazonService amazonService)
        {
            _context = context;
            _amazonService = amazonService;
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
    }
}
