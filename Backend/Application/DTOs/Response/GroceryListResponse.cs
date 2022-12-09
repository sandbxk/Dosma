﻿using Domain;

namespace Application.DTOs.Response;

public class GroceryListResponse
{
    public int Id { get; set; }
    public String Title { get; set; }
    public List<UserDTO> Users { get; set; }
    public List<Item> Items { get; set; }
}