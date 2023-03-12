using Core.Application.Dto;
using Core.Application.Dto.auth;
using Core.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace luftbornCodeTestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _auth;
        public AuthController(IAuthService auth)
        {
            _auth = auth;
        }
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync(RegisterDto model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            ResponseDto result = await _auth.RegisterAsync(model);
            if (!result.Status) return BadRequest(result);
            return Ok(result);
        }
        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync(LoginCredentialsDto model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            ResponseDto result = await _auth.GetTokenAsync(model);
            if (!result.Status) return BadRequest(result);
            return Ok(result);
        }
        [HttpGet("confirmEmail")]
        public async Task<IActionResult> ConfirmEmailAsync(string userId, string token)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(token)) return NotFound();
            ResponseDto result = await _auth.ConfirmEmailAsync(userId, token);
            if (!result.Status) return BadRequest(result);
            return Ok(result);
        }
        [HttpPost("forgetPassword")]
        public async Task<IActionResult> ForgetPasswordAsync(string email)
        {
            if (string.IsNullOrEmpty(email)) return NotFound();
            ResponseDto result = await _auth.ForgetPasswordAsync(email);
            if (!result.Status) return BadRequest(result);
            return Ok(result);
        }
        [HttpPost("resetPassword")]
        public async Task<IActionResult> ResetPasswordAsync(ResetPassowrdDto model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            ResponseDto result = await _auth.ResetPasswordAsync(model);
            if (!result.Status) return BadRequest(result);
            return Ok(result);
        }
        [Authorize]
        [HttpGet("profile")]
        public async Task<IActionResult> ProfileAsync(string id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            ResponseDto result = await _auth.GetUserById(id);
            if (!result.Status) return BadRequest(result);
            return Ok(result);
        }
        [Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdateAsync(string id, UserDto model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            ResponseDto res = await _auth.EditUser(id, model);
            if (!res.Status) return BadRequest(res);
            return Ok(res);
        }
        [Authorize]
        [HttpPut("changePassword")]
        public async Task<IActionResult> ChangePasswordAsync(string id, UpdatePassowrdDto model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            ResponseDto res = await _auth.ChangePasswordAsync(id, model);
            if (!res.Status) return BadRequest(res);
            return Ok(res);
        }

    }
}
