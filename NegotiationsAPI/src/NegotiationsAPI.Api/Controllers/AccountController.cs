using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NegotiationsAPI.Api.Dtos;
using NegotiationsAPI.Core.Enums;
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
        private IRoleService _roleService;
        public AccountController(IUserService userService, IRoleService roleService)
        {
            _userService = userService;
            _roleService = roleService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            var result = await _userService.RegisterUserAsync(model.Email, model.Password);

            if (result.Succeeded)
            {
                await _roleService.AssignRoleToUserAsync(model.Email, model.Role);
                return Ok();
            }
            return BadRequest(result.Errors);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
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
