using CleverCode.DTO;
using CleverCode.Helpers;
using CleverCode.Helpers.Error_Response;
using CleverCode.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CleverCode.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamMembersController : ControllerBase
    {
        private readonly ITeamMemberService _teamMemberService;

        public TeamMembersController(ITeamMemberService teamMemberService)
        {
            _teamMemberService = teamMemberService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTeamMembers()
        {
            var result = await _teamMemberService.GetAllTeamMembersAsync();
            if (!result.Success)
                return BadRequest(new ErrorResponse(result.StatusCode, result.Message));
            return Ok(new BaseResponse(result.StatusCode, result.Data, result.Message));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTeamMemberById(int id)
        {
            var result = await _teamMemberService.GetTeamMemberByIdAsync(id);
            if (result == null || !result.Success)
                return NotFound(new ErrorResponse(result?.StatusCode ?? HttpStatusCode.NotFound, result?.Message ?? "Not found"));
            return Ok(new BaseResponse(result.StatusCode, result.Data, result.Message));
        }

        [HttpPost]
        public async Task<IActionResult> CreateTeamMember([FromBody] TeamMemberDto teamMemberDto)
        {
            var result = await _teamMemberService.CreateTeamMemberAsync(teamMemberDto);
            if (!result.Success)
                return BadRequest(new ErrorResponse(result.StatusCode, result.Message));
            return Ok(new BaseResponse(result.StatusCode, result.Data, result.Message));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTeamMember(int id, [FromBody] TeamMemberDto teamMemberDto)
        {
            if (teamMemberDto.TeamMember_ID != id)
                return BadRequest(new ErrorResponse(HttpStatusCode.BadRequest, "Team member ID mismatch."));
            var result = await _teamMemberService.UpdateTeamMemberAsync(id, teamMemberDto);
            if (!result.Success)
                return BadRequest(new ErrorResponse(result.StatusCode, result.Message));
            return Ok(new BaseResponse(result.StatusCode, result.Data, result.Message));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTeamMember(int id)
        {
            var result = await _teamMemberService.DeleteTeamMemberAsync(id);
            if (!result.Success)
                return BadRequest(new ErrorResponse(result.StatusCode, result.Message));
            return Ok(new BaseResponse(result.StatusCode, result.Data, result.Message));
        }
    }
}
