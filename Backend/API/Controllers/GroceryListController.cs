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