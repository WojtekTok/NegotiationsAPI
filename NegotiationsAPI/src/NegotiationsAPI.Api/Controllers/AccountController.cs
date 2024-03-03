using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NegotiationsAPI.Api.Dtos;
using NegotiationsAPI.Core.Interfaces;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NegotiationsAPI.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private IUserService _userService;
        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserDto model)
        {
            var result = await _userService.RegisterUserAsync(model.Email, model.Password);

            if (result.Succeeded)
            {
                return Ok();
            }
            return BadRequest(result.Errors);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserDto model)
        {
            var token = await _userService.LoginUserAsync(model.Email, model.Password);

            if (token != null)
            {
                return Ok(new { Token = token });
            }
            return Unauthorized();
        }
    }
}
