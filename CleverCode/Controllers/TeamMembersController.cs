using CleverCode.DTO;
using CleverCode.Helpers;
using CleverCode.Helpers.Error_Response;
using CleverCode.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CleverCode.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamMembersController : ControllerBase
    {
        private readonly ITeamMemberService _service;

        public TeamMembersController(ITeamMemberService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllTeamMembersAsync();
            if (!result.Success) return BadRequest(new ErrorResponse(result.StatusCode, result.Message));
            return Ok(new BaseResponse(result.StatusCode, result.Data, result.Message));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetTeamMemberByIdAsync(id);
            if (result == null || !result.Success)
                return NotFound(new ErrorResponse(result?.StatusCode ?? HttpStatusCode.NotFound, result?.Message ?? "Not found"));

            return Ok(new BaseResponse(result.StatusCode, result.Data, result.Message));
        }

        [Authorize(Roles = "Admin,super-Admin", AuthenticationSchemes = "Bearer")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TeamMemberDto dto)
        {
            var result = await _service.CreateTeamMemberAsync(dto);
            if (!result.Success) return BadRequest(new ErrorResponse(result.StatusCode, result.Message));
            return Ok(new BaseResponse(result.StatusCode, result.Data, result.Message));
        }

        [Authorize(Roles = "Admin,super-Admin", AuthenticationSchemes = "Bearer")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] TeamMemberDto dto)
        {
            if (dto.TeamMember_ID != id)
                return BadRequest(new ErrorResponse(HttpStatusCode.BadRequest, "ID mismatch"));

            var result = await _service.UpdateTeamMemberAsync(id, dto);
            if (!result.Success) return BadRequest(new ErrorResponse(result.StatusCode, result.Message));
            return Ok(new BaseResponse(result.StatusCode, result.Data, result.Message));
        }

        [Authorize(Roles = "Admin,super-Admin", AuthenticationSchemes = "Bearer")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteTeamMemberAsync(id);
            if (!result.Success) return BadRequest(new ErrorResponse(result.StatusCode, result.Message));
            return Ok(new BaseResponse(result.StatusCode, result.Data, result.Message));
        }
    }
}
