using Domain;

namespace Application.Interfaces;

public interface IItemService
{
    public Item AddItemToList(Item item);
    public Item RemoveItemFromList(Item item);
    public Item UpdateItemInList(Item item);
}