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
    public ActionResult<List<GroceryListResponse>> GetAllLists([FromHeader] string token)
    {
        if (_authenticationService.AuthenticateToken(token))
        {
            // TODO: Check authorization
                // Forbid();

            var user = _authenticationService.GetUserFromToken(token);

            // return all lists for user 

            return Ok(_groceryListService.GetAllLists());
        }

        return Unauthorized("Could not be authenticated");
    }

    [HttpGet("{id}")]
    public ActionResult<List<GroceryListResponse>> GetListsByUser(int id, [FromHeader] string token)
    {
        if (_authenticationService.AuthenticateToken(token))
        {
            // TODO: Check authorization
                // id matches a list that the user has access to
                // Forbid();

            var user = _authenticationService.GetUserFromToken(token);

            if (user == null)
            {
                return NotFound("User not found");
            }
            
            //return Ok(_groceryListService.GetListsByUser(user));
            return Ok(_groceryListService.GetListsByUser(user));
        }

        return Unauthorized("Could not be authenticated");
    }

    [HttpPost]
    public ActionResult<GroceryListResponse> CreateGroceryList(GroceryListCreateRequest dto, [FromHeader] string token)
    {
        if (_authenticationService.AuthenticateToken(token))
        {
            try
            {
                // TODO: Check authorization
                    // Forbid();

                var user = _authenticationService.GetUserFromToken(token);
                // TODO: bind user to list as owner

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
    public ActionResult<GroceryListResponse> UpdateList([FromRoute] int id, [FromBody] GroceryList groceryList, [FromHeader] string token)
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
                    // id matches a list that the user has access to and has permission to edit
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
                    // id matches a list that the user has access to and has permission to delete
                    // Forbid();
                    
                return Ok(_groceryListService.DeleteList(id));
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