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
    public ActionResult<GroceryList> CreateGroceryList(GroceryListDTO dto)
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

    [HttpPatch]
    [Route("{id}")]
    public ActionResult<GroceryList> UpdateList([FromRoute] int id, [FromBody] GroceryList groceryList)
    {
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
    
    [HttpDelete]
    [Route("{id}")]
    public ActionResult DeleteList([FromRoute] int id, [FromBody] GroceryList groceryList)
    {
        try
        {
            return Ok(_groceryListService.DeleteList(id, groceryList));
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