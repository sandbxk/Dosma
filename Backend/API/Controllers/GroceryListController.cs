using Application.DTOs;
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
    
    public GroceryListController(IGroceryListService groceryListService)
    {
        _groceryListService = groceryListService;
    }

    
    /**
     * Get Lists by various methods
     */
    [HttpGet]
    public IEnumerable<GroceryList> GetAllLists()
    {
        return _groceryListService.GetAllLists();
    }

    [HttpGet("{id}")]
    public IEnumerable<GroceryList> GetListsByUser([FromBody] User user)
    {
        return _groceryListService.GetListsByUser(user);
    }

    [HttpPost]
    public ActionResult<GroceryList> CreateGroceryList([FromBody] GroceryListDTO dto)
    {
        try
        {
            var result = _groceryListService.Create(dto);
            return Created("GroceryList/" + result.Id, result);
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
    public ActionResult<GroceryList> DeleteList([FromRoute] int id, [FromBody] GroceryList groceryList)
    {
        try
        {
            return Ok(_groceryListService.DeleteList(groceryList, id));
        }
        catch (KeyNotFoundException e)
        {
            return NotFound("No List with id: " + id);
        }
        catch (Exception e)
        {
            return (StatusCode(500, e.ToString()));
        }
    }

}
