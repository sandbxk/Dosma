using Application.DTOs;
using Domain;

namespace Application.Interfaces;

public interface IItemService
{
    public Item AddItemToList(ItemDTO itemDTO);
    public Item RemoveItemFromList(Item item);
    public Item UpdateItemInList(Item item);
}