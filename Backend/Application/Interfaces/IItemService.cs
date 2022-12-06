using Application.DTOs;
using Domain;

namespace Application.Interfaces;

public interface IItemService
{
    public Item AddItem(ItemDTO itemDTO);
    public bool DeleteItem(Item item);
    public Item UpdateItem(Item item);
}