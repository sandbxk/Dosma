using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.API.Controllers;

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
    public IActionResult Login([FromBody] LoginRequestDTO loginDTO)
    {
        if (_authenticationService.ValidateLogin(loginDTO, out string result))
        {
            return Ok(new TokenResponseDTO { Token = result });
        }

        return BadRequest(new TokenResponseDTO { Status = 400, ErrorMessage = result });
    }
}