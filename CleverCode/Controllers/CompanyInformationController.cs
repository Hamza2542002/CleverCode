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
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyInformationController : ControllerBase
    {
        private readonly ICompanyInformationService _service;

        public CompanyInformationController(ICompanyInformationService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(new BaseResponse(HttpStatusCode.OK, result, "Company information retrieved successfully."));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null)
            {
                return NotFound(new ErrorResponse(HttpStatusCode.NotFound, "Company not found."));
            }

            return Ok(new BaseResponse(HttpStatusCode.OK, item, "Company retrieved successfully."));
        }

        [Authorize(Roles = "Admin", AuthenticationSchemes = "Bearer")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CompanyInformationDto dto)
        {
            var created = await _service.CreateAsync(dto);
            if (created == null)
                return BadRequest(new ErrorResponse(HttpStatusCode.BadRequest, "Failed to create company."));

            return Ok(new BaseResponse(HttpStatusCode.Created, created, "Company created successfully."));
        }

        [Authorize(Roles = "Admin", AuthenticationSchemes = "Bearer")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CompanyInformationDto dto)
        {
            var updated = await _service.UpdateAsync(id, dto);
            if (!updated)
            {
                return NotFound(new ErrorResponse(HttpStatusCode.NotFound, "Company not found."));
            }

            return Ok(new BaseResponse(HttpStatusCode.OK, null, "Company updated successfully."));
        }

        [Authorize(Roles = "Admin", AuthenticationSchemes = "Bearer")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted)
            {
                return NotFound(new ErrorResponse(HttpStatusCode.NotFound, "Company not found."));
            }

            return Ok(new BaseResponse(HttpStatusCode.OK, null, "Company deleted successfully."));
        }
    }
}
