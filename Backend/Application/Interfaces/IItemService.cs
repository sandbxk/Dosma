using Application.DTOs;
using Domain;

namespace Application.Interfaces;

public interface IItemService
{
    public Item AddItemToList(ItemDTO itemDTO);
    public bool DeleteItemFromList(int id, Item item);
    public Item UpdateItem(Item item);
}