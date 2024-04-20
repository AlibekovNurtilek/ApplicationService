using ApplicationService.BLL.Models.GradeModels;
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
    public class GradeController : ControllerBase
    {
        private readonly IGradeService _service;
        private readonly UserManager<ApplicationUser> _userManager;
        public GradeController(IGradeService service, UserManager<ApplicationUser> userManager)
        {
            _service = service;
            _userManager = userManager;
        }
        [HttpGet]
        [Route("GetSingleGrade")]

        public async Task<ActionResult<GradeResponse>> GetSingleGrade([FromQuery] int id)
        {
            if (id <= 0) { return BadRequest("ID cannot be less than 0 or equal"); }
            var result = await _service.GetSingleGrade(id);
            return result == null ? NotFound("No data found from the database!") : Ok(result);
        }
        [Route("GetAllGrade")]
        [HttpGet]
        [Authorize]

        public async Task<ActionResult<List<GradeResponse>>> GetAllGrade()
        {
            var result = await _service.GetAllGrade();
            return result == null ? NotFound("No data found from the database!") : Ok(result);
        }
        [Route("CreateGrade")]
        [HttpPost]

        public async Task<ActionResult<ApiResponse>> CreateGrade([FromBody] GradeRequest model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            //if (currentUserId == null)
            //{
            //    return Unauthorized("UserID Not found");
            //}
            if (model == null) { return NoContent(); }
            var result = await _service.CreateGrade("bce43e26-16ac-4219-869e-31153b1c721f", model);
            if (result.Success) { return Ok(result); }
            return BadRequest(result);
        }
        [Route("DeleteGrade")]
        [HttpDelete]

        public async Task<ActionResult<ApiResponse>> DeleteGrade([FromQuery] int id)
        {
            var result = await _service.DeleteGrade(id);
            if (result.Success)
            {
                return Ok(result);
            }
            return NotFound(result);
        }
        [Route("UpdateGrade")]
        [HttpPut]

        public async Task<ActionResult<ApiResponse>> UpdateGrade([FromQuery] int id, [FromBody] GradeRequest model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (model == null) { return NoContent(); }
            var result = await _service.UpdateGrade(id, model);
            if (result.Success)
            {
                return Ok(result);
            }
            return NotFound(result);
        }
    }
}
