using CleverCode.DTO;
using CleverCode.Helpers;
using CleverCode.Helpers.Error_Response;
using CleverCode.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace CleverCode.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ComplaintController : ControllerBase
    {
        private readonly IComplaintService _service;

        public ComplaintController(IComplaintService service)
        {
            _service = service;
        }

        [Authorize(Roles = "Admin", AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? lang = "en")
        {
            var data = await _service.GetAllAsync(lang);
            return Ok(new BaseResponse(HttpStatusCode.OK, data, "Complaints retrieved successfully."));
        }

        [Authorize(Roles = "Admin", AuthenticationSchemes = "Bearer")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id, [FromQuery] string? lang = "en")
        {
            var item = await _service.GetByIdAsync(id, lang);
            if (item == null)
                return NotFound(new ErrorResponse(HttpStatusCode.NotFound, "Complaint not found."));
            return Ok(new BaseResponse(HttpStatusCode.OK, item, "Complaint retrieved successfully."));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ComplaintDto dto, [FromQuery] string? lang = "en")
        {
            var created = await _service.CreateAsync(dto, lang);
            return CreatedAtAction(nameof(GetById), new { id = created.Complaint_ID, lang }, new BaseResponse(HttpStatusCode.Created, created, "Complaint created successfully."));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ComplaintDto dto)
        {
            var updated = await _service.UpdateAsync(id, dto);
            if (!updated)
                return NotFound(new ErrorResponse(HttpStatusCode.NotFound, "Complaint not found."));
            return Ok(new BaseResponse(HttpStatusCode.OK, null, "Complaint updated successfully."));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted)
                return NotFound(new ErrorResponse(HttpStatusCode.NotFound, "Complaint not found."));
            return Ok(new BaseResponse(HttpStatusCode.OK, null, "Complaint deleted successfully."));
        }
    }
}
