using ApplicationService.DAL.Contexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApplicationService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SecretarController : ControllerBase
    {
        private readonly AppDbContext _appContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public SecretarController(AppDbContext appContext, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _appContext = appContext;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [Authorize]
        [HttpGet]
        [Route("GetAllStatementsBydepartment")]

        public async Task<IActionResult> GetAllStatements()
        {
            try
            {
                var currentUSerID = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

                if (currentUSerID == null)
                    return NotFound("Secretar not found");
                var secretar = await _userManager.FindByIdAsync(currentUSerID);
                if (secretar == null)
                    return NotFound("Secretar not found");

                var statements = await _appContext.Statements.Where(u => u.DepartmentId == secretar.DepartmentId && u.IsAccespted == false).ToListAsync();

                //var statements = await _userManager.Users.Where(u => u.DepartmentId == secretar.DepartmentId).ToListAsync();
                return Ok(statements);
            }
            catch (Exception ex)
            {
                // Возврат ошибки сервера с текстом ошибки
                return StatusCode(500, "Internal server error: " + ex.Message);
            }

        }


    }
}
