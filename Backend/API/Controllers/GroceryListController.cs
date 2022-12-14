using System.Data;
using Application.DTOs;
using Application.DTOs.Response;
using Application.Helpers;
using Application.Interfaces;
using Domain;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class GroceryListController : ControllerBase
{
    private readonly IGroceryListService _groceryListService;
    private readonly IAuthenticationService _authenticationService;
    
    public GroceryListController(IGroceryListService groceryListService, IAuthenticationService authenticationService)
    {
        _groceryListService = groceryListService;
        _authenticationService = authenticationService;
    }
    
    [Produces("application/json")]
    [HttpGet]
    public List<GroceryListResponse> GetListsByUser([FromHeader] String token)
    {
        if (!_authenticationService.AuthenticateToken(token))
            throw new UnauthorizedAccessException();
        
        var user = _authenticationService.GetPartialUserFromToken(token);
        
        if (user == null)
        {
            throw new NullReferenceException("User could not be found.");
        }
        
        return _groceryListService.GetListsByUser(user);
    }
    
    [Produces("application/json")]
    [HttpGet]
    [Route("{id}")]
    public ActionResult<GroceryListResponse> GetListById([FromRoute] int id, [FromHeader] String token)
    {
        if (!_authenticationService.AuthenticateToken(token))
            throw new UnauthorizedAccessException();
        
        var user = _authenticationService.GetPartialUserFromToken(token);
        
        if (user == null)
        {
            throw new NullReferenceException("User could not be found.");
        }
        
        if (id == 0 || id < 0)
        {
            return BadRequest("Invalid id.");
        }

        try
        {
            return Ok(_groceryListService.GetListById(id));
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
    
    [Produces("application/json")]
    [HttpPost]
    public ActionResult<GroceryListResponse> CreateGroceryList(GroceryListCreateRequest request, [FromHeader] String token)
    {
        if (!_authenticationService.AuthenticateToken(token))
            throw new UnauthorizedAccessException();
        
        var user = _authenticationService.GetPartialUserFromToken(token);
        
        if (user == null)
        {
            throw new NullReferenceException("User could not be found.");
        }
        
        try
        {
            var result = _groceryListService.Create(request, user);
            return Created("product/" + result.Id, result);
        }
        catch (ValidationException e)
        {
            return BadRequest(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [Consumes("application/json")]
    [Produces("application/json")]
    [HttpPatch]
    [Route("{id}")]
    public ActionResult<GroceryListResponse> UpdateList([FromRoute] int id, [FromBody] GroceryListUpdateRequest groceryList, [FromHeader] String token)
    {
        if (!_authenticationService.AuthenticateToken(token))
            throw new UnauthorizedAccessException();
        
        var user = _authenticationService.GetPartialUserFromToken(token);

        if (user == null)
        {
            throw new NullReferenceException("User could not be found.");
        }
        
        if (id != groceryList.Id)
        {
            throw new ValidationException("List ID does not match ID in URL.");
        }
        
        try
        {
            return Ok(_groceryListService.UpdateList(groceryList));
        }
        catch (ValidationException e)
        {
            return BadRequest(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
    
    [Consumes("application/json")]
    [Route("{id}")]
    [HttpDelete]
    public ActionResult DeleteList([FromRoute] int id, [FromHeader] String token)
    {
        if (!_authenticationService.AuthenticateToken(token))
            throw new UnauthorizedAccessException();
        
        var user = _authenticationService.GetPartialUserFromToken(token);
        
        if (user == null)
        {
            throw new NullReferenceException("User could not be found.");
        }
        
        try
        {
            _groceryListService.DeleteList(id, user);
            return Ok("List has been deleted.");
        }
        catch (ValidationException e)
        {
            return BadRequest(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
}