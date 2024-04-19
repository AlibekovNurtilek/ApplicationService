using ApplicationService.BLL.Models.GlobalExamModels;
using ApplicationService.BLL.Models.Responses;
using ApplicationService.BLL.Services;
using ApplicationService.DAL.Contexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GlobalExamController : ControllerBase
    {
        private readonly IGlobalExamService _service;
        private readonly UserManager<ApplicationUser> _userManager;
        public GlobalExamController(IGlobalExamService service, UserManager<ApplicationUser> userManager)
        {
            _service = service;
            _userManager = userManager;
        }
        [HttpGet]
        [Route("GetSingleGlobalExam")]

        public async Task<ActionResult<GlobalExamResponse>> GetSingleGlobalExam([FromQuery] int id)
        {
            if (id <= 0) { return BadRequest("ID cannot be less than 0 or equal"); }
            var result = await _service.GetSingleGlobalExam(id);
            return result == null ? NotFound("No data found from the database!") : Ok(result);
        }
        [Route("GetAllGlobalExam")]
        [HttpGet]

        public async Task<ActionResult<List<GlobalExamResponse>>> GetAllGlobalExam()
        {
            var result = await _service.GetAllGlobalExam();
            return result == null ? NotFound("No data found from the database!") : Ok(result);
        }
        [Authorize]
        [Route("CreateGlobalExam")]
        [HttpPost]

        public async Task<ActionResult<ApiResponse>> CreateGlobalExam([FromBody] GlobalExamRequest model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            
            if (currentUserId == null)
            {
                return Unauthorized("UserID Not found");
            }
            var user = await _userManager.FindByIdAsync(currentUserId);
            if (model == null) { return NoContent(); }
            var result = await _service.CreateGlobalExam(user.DepartmentId, model);
            if (result.Success) { return Ok(result); }
            return BadRequest(result);
        }
        [Route("DeleteGlobalExam")]
        [HttpDelete]

        public async Task<ActionResult<ApiResponse>> DeleteGlobalExam([FromQuery] int id)
        {
            var result = await _service.DeleteGlobalExam(id);
            if (result.Success)
            {
                return Ok(result);
            }
            return NotFound(result);
        }
        [Route("UpdateGlobalExam")]
        [HttpPut]

        public async Task<ActionResult<ApiResponse>> UpdateGlobalExam([FromQuery] int id, [FromBody] GlobalExamRequest model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (model == null) { return NoContent(); }
            var result = await _service.UpdateGlobalExam(id, model);
            if (result.Success)
            {
                return Ok(result);
            }
            return NotFound(result);
        }
    }
}
