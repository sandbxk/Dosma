using System.Data;
using Application.DTOs;
using Application.Interfaces;
using Domain;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
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

    
    /**
     * Get Lists by various methods
     */
    [HttpGet]
    public ActionResult<IEnumerable<GroceryList>> GetAllLists([FromHeader] string token)
    {
        if (_authenticationService.AuthenticateToken(token))
        {
            // TODO: Check authorization
                // Forbid();

            return Ok(_groceryListService.GetAllLists());
        }

        return Unauthorized("Could not be authenticated");
    }

    [HttpGet("{id}")]
    public ActionResult<IEnumerable<GroceryList>> GetListsByUser(int id, [FromBody] User user, [FromHeader] string token)
    {
        if (_authenticationService.AuthenticateToken(token))
        {
            // TODO: Check authorization
                // Forbid();

            //return Ok(_groceryListService.GetListsByUser(user));
            return Ok(_groceryListService.GetListsByUser(user));
        }

        return Unauthorized("Could not be authenticated");
    }

    [HttpPost]
    public ActionResult<GroceryList> CreateGroceryList(GroceryListDTO dto, [FromHeader] string token)
    {
        if (_authenticationService.AuthenticateToken(token))
        {
            try
            {
                // TODO: Check authorization
                    // Forbid();

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

        return Unauthorized("Could not be authenticated");
    }

    [HttpPatch]
    [Route("{id}")]
    public ActionResult<GroceryList> UpdateList([FromRoute] int id, [FromBody] GroceryList groceryList, [FromHeader] string token)
    {
        if (_authenticationService.AuthenticateToken(token))
        {
            try
            {
                if (id != groceryList.Id)
                {
                    throw new ValidationException("List ID does not match ID in URL.");
                }

                // TODO: Check authorization
                    // Forbid();

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

        return Unauthorized("Could not be authenticated");
    }
    
    [HttpDelete]
    [Route("{id}")]
    public ActionResult DeleteList([FromRoute] int id, [FromBody] GroceryList groceryList, [FromHeader] string token)
    {
        if (_authenticationService.AuthenticateToken(token))
        {
            try
            {
                if (id != groceryList.Id)
                {
                    throw new ValidationException("List ID does not match ID in URL.");
                }
                
                // TODO: Check authorization
                    // Forbid();
                    
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

        return Unauthorized("Could not be authenticated");
    }
}