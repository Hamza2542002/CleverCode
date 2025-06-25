using CleverCode.DTO;
using CleverCode.Helpers;
using CleverCode.Helpers.Error_Response;
using CleverCode.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace CleverCode.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicesController : ControllerBase
    {
        private readonly IServicesService _servicesService;
        public ServicesController(IServicesService servicesService)
        {
            _servicesService = servicesService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllServices()
        {
            var result = await _servicesService.GetAllServicesAsync();
            if (!result.Success)
                return BadRequest(new ErrorResponse(result.StatusCode, result.Message));
            return Ok(new BaseResponse( result.StatusCode, result.Data, result.Message ));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetServiceById(int id)
        {
            var result = await _servicesService.GetServiceByIdAsync(id);
            if (!result.Success)
                return NotFound(new ErrorResponse(result.StatusCode, result.Message));
            return Ok(new BaseResponse( result.StatusCode, result.Data, result.Message ));
        }

        [HttpPost]
        public async Task<IActionResult> CreateService([FromBody] ServiceDto serviceDto)
        {
            var result = await _servicesService.CreateServiceAsync(serviceDto);
            if (!result.Success)
                return BadRequest(new ErrorResponse(result.StatusCode, result.Message));
            return Ok(new BaseResponse( result.StatusCode, result.Data, result.Message ));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateService(int id, [FromBody] ServiceDto serviceDto)
        {
            if (serviceDto.Service_ID != id)
                return BadRequest(new ErrorResponse(HttpStatusCode.BadRequest, "Service ID mismatch."));
            var result = await _servicesService.UpdateServiceAsync(id, serviceDto);
            if (!result.Success)
                return BadRequest(new ErrorResponse(result.StatusCode, result.Message));
            return Ok(new BaseResponse( result.StatusCode, result.Data, result.Message ));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteService(int id)
        {
            var result = await _servicesService.DeleteServiceAsync(id);
            if (!result.Success)
                return BadRequest(new ErrorResponse(result.StatusCode, result.Message));
            return Ok(new BaseResponse( result.StatusCode,null,result.Message));
        }
    }
}
