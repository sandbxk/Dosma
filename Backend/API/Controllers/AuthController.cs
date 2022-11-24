using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login([FromBody] LoginDTO loginDTO)
        {
            if (_authenticationService.ValidateLogin(loginDTO.Username, loginDTO.Password, out string token))
            {
                return Ok(new TokenDTO { Token = token });
            }

            return BadRequest(new TokenDTO { Status = 400, ErrorMessage = "Invalid username or password" });
        }
    }
}