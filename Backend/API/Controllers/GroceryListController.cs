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
    private IGroceryListService _groceryListService;
    
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
    public ActionResult<GroceryListController> CreateGroceryList(GroceryListDTO dto)
    {
        try
        {
            var result = _groceryListService.Create(dto);
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

    [HttpDelete]
    [Route("{id}")]
    public ActionResult<GroceryList> DeleteList([FromRoute] int id)
    {
        try
        {
            return Ok(_groceryListService.DeleteList(id));
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