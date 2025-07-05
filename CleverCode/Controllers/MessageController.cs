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
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _service;

        public MessageController(IMessageService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _service.GetAllAsync();
            return Ok(new BaseResponse(HttpStatusCode.OK, data, "Messages retrieved successfully."));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null)
                return NotFound(new ErrorResponse(HttpStatusCode.NotFound, "Message not found."));

            return Ok(new BaseResponse(HttpStatusCode.OK, item, "Message retrieved successfully."));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] MessageDto dto)
        {
            var created = await _service.CreateAsync(dto);
            if (created == null)
                return BadRequest(new ErrorResponse(HttpStatusCode.BadRequest, "Failed to create message."));

            return Ok(new BaseResponse(HttpStatusCode.Created, created, "Message created successfully."));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] MessageDto dto)
        {
            var updated = await _service.UpdateAsync(id, dto);
            if (!updated)
                return NotFound(new ErrorResponse(HttpStatusCode.NotFound, "Message not found."));

            return Ok(new BaseResponse(HttpStatusCode.OK, dto, "Message updated successfully."));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted)
                return NotFound(new ErrorResponse(HttpStatusCode.NotFound, "Message not found."));

            return Ok(new BaseResponse(HttpStatusCode.OK, null, "Message deleted successfully."));
        }
    }
}
