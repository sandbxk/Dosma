﻿using System.ComponentModel.DataAnnotations;
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
    [Route("{id}")]
    public ActionResult<Item> UpdateItem([FromRoute] int id, [FromBody] Item item)
    {
        if (id != item.Id)
            throw new ValidationException("List ID does not match ID in URL.");
        
        try
        {
            var result = _itemService.UpdateItem(item);
            return Ok(result);
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
            var result = _itemService.DeleteItemFromList(id, item);
            return Ok(item.Title + " has been deleted.");
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
