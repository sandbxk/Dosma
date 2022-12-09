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
    public List<GroceryList> GetListsByUser([FromHeader] String token)
    {
        var user = _authenticationService.GetUserFromToken(token);
        
        if (user == null)
        {
            throw new NullReferenceException("User could not be found.");
        }
        
        return _groceryListService.GetListsByUser(user);
    }
    
    [Produces("application/json")]
    [HttpGet]
    [Route("grocerylist/{id}")]
    public ActionResult<GroceryList> GetListById([FromRoute] int id, [FromHeader] String token)
    {
        var user = _authenticationService.GetUserFromToken(token);
        
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
    public ActionResult<GroceryListResponse> CreateGroceryList(GroceryListRequest request, [FromHeader] String token)
    {
        var user = _authenticationService.GetUserFromToken(token);
        
        if (user == null)
        {
            throw new NullReferenceException("User could not be found.");
        }
        
        try
        {
            var result = _groceryListService.Create(request);
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
    public ActionResult<GroceryList> UpdateList([FromRoute] int id, [FromBody] GroceryList groceryList, [FromHeader] String token)
    {
        var user = _authenticationService.GetUserFromToken(token);

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
            return Ok(_groceryListService.UpdateList(id, groceryList));
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
    [HttpDelete]
    public ActionResult DeleteList([FromBody] GroceryList groceryList, [FromHeader] String token)
    {
        var user = _authenticationService.GetUserFromToken(token);
        
        if (user == null)
        {
            throw new NullReferenceException("User could not be found.");
        }
        
        try
        {
            return Ok(_groceryListService.DeleteList(groceryList));
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