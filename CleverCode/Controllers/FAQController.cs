using CleverCode.DTO;
using CleverCode.Helpers;
using CleverCode.Helpers.Error_Response;
using CleverCode.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace CleverCode.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FAQController : ControllerBase
    {
        private readonly IFAQService _service;

        public FAQController(IFAQService service)
        {
            _service = service;
        }

        private string? GetLanguageFromHeader()
        {
            var langHeader = Request.Headers["Accept-Language"].ToString();
            return string.IsNullOrEmpty(langHeader) ? "en" : langHeader.ToLower();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(new BaseResponse(HttpStatusCode.OK, result, "FAQs retrieved successfully."));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null)
                return NotFound(new ErrorResponse(HttpStatusCode.NotFound, $"FAQ with ID {id} not found."));

            return Ok(new BaseResponse(HttpStatusCode.OK, result, "FAQ retrieved successfully."));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] FAQDto dto)
        {
            var created = await _service.CreateAsync(dto);
            if (created == null)
                return BadRequest(new ErrorResponse(HttpStatusCode.BadRequest, "Failed to create FAQ."));

            return Ok(new BaseResponse(HttpStatusCode.Created, created, "FAQ created successfully."));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] FAQDto dto)
        {
            var updated = await _service.UpdateAsync(id, dto);
            if (!updated)
                return NotFound(new ErrorResponse(HttpStatusCode.NotFound, $"FAQ with ID {id} not found."));

            return Ok(new BaseResponse(HttpStatusCode.OK, dto, "FAQ updated successfully."));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted)
                return NotFound(new ErrorResponse(HttpStatusCode.NotFound, $"FAQ with ID {id} not found."));

            return Ok(new BaseResponse(HttpStatusCode.OK, null, "FAQ deleted successfully."));
        }
    }
}
