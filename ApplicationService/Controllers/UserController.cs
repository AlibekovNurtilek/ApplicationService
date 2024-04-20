using ApplicationService.DAL.Contexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _appContext;
        private readonly UserManager<ApplicationUser> _userManager;
        public UserController(AppDbContext appContext, UserManager<ApplicationUser> userManager)
        {
            _userManager= userManager;
            _appContext = appContext;
        }
        [Authorize]
        [Route("GetCurrentUser")]
        [HttpGet]
        public async Task<IActionResult> GetCurrentUser()
        {
            var currentUSerID = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (currentUSerID == null)
                return NotFound("Secretar not found");
            var currentUser = await _userManager.FindByIdAsync(currentUSerID);
            if (currentUser == null)
                return NotFound("Secretar not found");
            return Ok(currentUser);
        }
    }
}
