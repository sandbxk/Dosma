using System.ComponentModel.DataAnnotations;
using Application.DTOs;
using Application.Interfaces;
using Domain;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class ItemController : ControllerBase
{
    private readonly IItemService _itemService;
    private readonly IAuthenticationService _authenticationService;

    public ItemController(IItemService itemService, IAuthenticationService authenticationService)
    {
        _itemService = itemService;
        _authenticationService = authenticationService;
    }

    [HttpPost]
    public ActionResult<Item> CreateItem([FromBody] Item item, [FromHeader] String token)
    {
        if (!_authenticationService.AuthenticateToken(token))
        {
            return Unauthorized("Invalid token");
        }
        
        var user = _authenticationService.GetPartialUserFromToken(token);
        
        if (user == null)
        {
            throw new NullReferenceException("User could not be found.");
        }

            try
        {
            var result = _itemService.AddItem(item);
            return Created("Item/" + result.Id, result);
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

    [HttpPatch]
    [Route("{id}")]
    public ActionResult<Item> UpdateItem([FromRoute] int id, [FromBody] Item item, [FromHeader] String token)
    {
        if (!_authenticationService.AuthenticateToken(token))
        {
            return Unauthorized("Invalid token");
        }
        
        var user = _authenticationService.GetPartialUserFromToken(token);
        
        if (user == null)
        {
            throw new NullReferenceException("User could not be found.");
        }
        
        if (id != item.Id)
            throw new ValidationException("Item ID does not match ID in URL.");
        
        try
        {
            var result = _itemService.UpdateItem(item);
            
            if (result == null)
            {
                return NotFound();
            }
            return Ok("Item has been updated.");
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
    
    
    [HttpDelete]
    [Route("{id}")]
    public ActionResult DeleteItem([FromRoute] int id, [FromHeader] String token)
    {
        if (!_authenticationService.AuthenticateToken(token))
        {
            return Unauthorized("Invalid token");
        }
        
        var user = _authenticationService.GetPartialUserFromToken(token);

        if (user == null)
        {
            throw new NullReferenceException("User could not be found.");
        }

        try
        {
            var result = _itemService.DeleteItem(id, user);
            return Ok("Item has been deleted.");
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
