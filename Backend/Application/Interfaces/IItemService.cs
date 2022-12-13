﻿using Application.DTOs;
using Domain;

namespace Application.Interfaces;

public interface IItemService
{
    public Item AddItem(ItemDTO itemDTO);
    public bool DeleteItem(int id, TokenUser user);
    public Item UpdateItem(Item item);
}