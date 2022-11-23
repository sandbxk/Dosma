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

    //[HttpGet]
    //[Route("{id}")]
    //public IEnumerable<GroceryList> GetListsByUser(User user)
    //{
    //    return _groceryListService.GetListsByUser(user);
    //}
    //
    //[HttpGet]
    //[Route("{id}")]
    //public GroceryList GetListbyId([FromRoute] int id)
    //{
    //    return _groceryListService.GetListById(id);
    //}
    
    
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

}