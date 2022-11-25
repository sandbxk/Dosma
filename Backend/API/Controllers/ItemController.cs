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
    public ActionResult<Item> CreateItem(ItemDTO item)
    {
        try
        {
            var result = _itemService.AddItemToList(item);
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
    
    
    [HttpDelete]
    [Route("{id}")]
    public ActionResult<Item> DeleteItem([FromRoute] int id, [FromBody] Item item)
    {
        try
        {
            var result = _itemService.DeleteItemFromList(id, item);
            return Ok(item.Title + " has been deleted.");
        }
        catch (ValidationException e)
        {
            return BadRequest(e.Message);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
}