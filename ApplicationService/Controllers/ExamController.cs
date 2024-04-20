using ApplicationService.BLL.Models.ExamModels;
using ApplicationService.BLL.Models.Responses;
using ApplicationService.BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Sockets;

namespace ApplicationService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamController : ControllerBase
    {
        private readonly IExamService _service;
        public ExamController(IExamService service)
        {
            _service = service;
        }
        [HttpGet]
        [Route("GetSingleExam")]
        
        public async Task<ActionResult<ExamResponse>> GetSingleExam([FromQuery] int id)
        {
            if (id <= 0) { return BadRequest("ID cannot be less than 0 or equal"); }
            var result = await _service.GetSingleExam(id);
            return result == null ? NotFound("No data found from the database!") : Ok(result);
        }
        [HttpGet]
        [Route("GetSingleExamById")]

        public async Task<ActionResult<ExamResponse>> GetSingleExamById([FromQuery] string id)
        {
            if (id == null) { return BadRequest("ID cannot be less than 0 or equal"); }
            var result = await _service.GetSingleExam(id);
            return result == null ? NotFound("No data found from the database!") : Ok(result);
        }
        [Route("GetAllExam")]
        [HttpGet]
        
        public async Task<ActionResult<List<ExamResponse>>> GetAllExam()
        {
            var result = await _service.GetAllExam();
            return result == null ? NotFound("No data found from the database!") : Ok(result);
        }

        [Route("CreateExam")]
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ApiResponse>> CreateExam([FromForm] ExamRequest model)
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
            if (model == null) { return NoContent(); }
            var result = await _service.CreateExam(currentUserId,model);
            if (result.Success) { return Ok(result); }
            return BadRequest(result);
        }
        [Route("DeleteExam")]
        [HttpDelete]
        
        public async Task<ActionResult<ApiResponse>> DeleteExam([FromQuery] int id)
        {
            var result = await _service.DeleteExam(id);
            if (result.Success)
            {
                return Ok(result);
            }
            return NotFound(result);
        }
        [Route("UpdateExam")]
        [HttpPut]
        
        public async Task<ActionResult<ApiResponse>> UpdateExam([FromQuery] int id,[FromBody] ExamRequest model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (model == null) { return NoContent(); }
            var result = await _service.UpdateExam(id,model);
            if (result.Success)
            {
                return Ok(result);
            }
            return NotFound(result);
        }
    }
}
