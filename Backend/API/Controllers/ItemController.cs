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

    public ItemController(IItemService itemService)
    {
        _itemService = itemService;
    }

    [HttpPost]
    public ActionResult<Item> CreateItem([FromBody] ItemDTO item)
    {
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
    public ActionResult<Item> UpdateItem([FromBody] Item item)
    {
        try
        {
            var result = _itemService.UpdateItem(item);
            
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result.Title + " has been updated.");
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
    public ActionResult DeleteItem([FromRoute] int id, [FromBody] Item item)
    {
        if (id != item.Id)
            throw new ValidationException("List ID does not match ID in URL.");
        try
        {
            var result = _itemService.DeleteItem(item);
            
            if (result)
                return Ok(item.Title + " has been deleted.");
            else
                return StatusCode(304, "Item could not be deleted.");
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
