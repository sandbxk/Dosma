using Application.DTOs;
using Application.Interfaces;
using Domain;
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
        _groceryListService.CreateGroceryList(dto);
        return Ok();
    }

}