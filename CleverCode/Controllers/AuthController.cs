using CleverCode.DTO;
using CleverCode.Interfaces;
using CleverCode.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleverCode.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthServices _authServices;

        public AuthController(IAuthServices authServices)
        {
            _authServices = authServices;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] TokenRequestModel model)
        {
            var result = await _authServices.GetTokenAsync(model);
            if (!result.IsAuthenticated)
                return Unauthorized(result);

            return Ok(result);
        }
        [Authorize(Roles = "super-Admin", AuthenticationSchemes = "Bearer")]
        [HttpPost("register-admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterModel model)
        {
            var result = await _authServices.RegisterAsync(model, isAdmin: true);
            if (!result.IsAuthenticated)
                return BadRequest(result);

            return Ok(result);
        }


        [Authorize(Roles = "super-Admin")]
        [HttpPost("create-admin")]
        public async Task<IActionResult> CreateAdmin([FromBody] RegisterModel model)
        {
            var result = await _authServices.RegisterAsync(model);
            if (!result.IsAuthenticated)
                return BadRequest(result);

            return Ok(result);
        }
        [Authorize(Roles = "super-Admin", AuthenticationSchemes = "Bearer")]
        [HttpPut("update-admin/{id}")]
        public async Task<IActionResult> UpdateAdmin(string id, [FromBody] UpdateAdminDto model)
        {
            var result = await _authServices.UpdateAdminAsync(id, model);
            if (!result.IsAuthenticated)
                return BadRequest(result);
            return Ok(result);
        }
        [HttpDelete("delete-admin/{id}")]
        [Authorize (Roles = "super-Admin", AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> DeleteAdmin(string id)
        {
            var result = await _authServices.DeleteAdminAsync(id);
            if (!result.IsAuthenticated)
                return BadRequest(result);
            return Ok(result);
        }
        [HttpGet("get-all-admins")]
        [Authorize(Roles = "super-Admin", AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetAllAdmins()
        {
            var result = await _authServices.GetAllAdminsAsync();
            return Ok(result);
        }
        [HttpGet("get-all-admins/{id}")]
        [Authorize(Roles = "super-Admin", AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetAdmin(string id)
        {
            var admin = await _authServices.GetAdminByIdAsync(id);
            if(admin == null)
                return NotFound(new { Message = "Admin not found" });
            return Ok(admin);
        }
    }
}
