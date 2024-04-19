using ApplicationService.BLL.Models.AuthModels;
using ApplicationService.BLL.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace ApplicationService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromForm]RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _authService.RegisterAsync(model);
                if(result.Success) return Ok(result);
                return BadRequest(result.Message);

            }
            return BadRequest(ModelState);
        }
    }
}
