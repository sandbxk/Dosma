
using Application.DTOs;
using Application.DTOs.Requests;
using Application.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.API.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;

    public AuthController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [AllowAnonymous]
    [Consumes("application/json")]
    [Produces("application/json")]
    [HttpPost("login")]
    public ActionResult Login([FromBody] LoginRequest loginInfo)
    {
        if (loginInfo == null)
            return BadRequest("No login information provided.");

        if (_authenticationService.Login(loginInfo, out string result))
        {
            return Ok(new TokenResponse { Token = result });
        }

        return BadRequest(new TokenResponse { Status = 400, ErrorMessage = result });
    }

    [AllowAnonymous]
    [Consumes("application/json")]
    [Produces("application/json")]
    [HttpPost("register")]
    public ActionResult Register([FromBody] RegisterRequest registrationInfo)
    {
        if (registrationInfo == null)
            return BadRequest("No registration information provided.");

        if (_authenticationService.Register(registrationInfo, out string result))
        {
            return Ok(new TokenResponse { Token = result });
        }

        return BadRequest(new TokenResponse { Status = 400, ErrorMessage = result });
    }
}
