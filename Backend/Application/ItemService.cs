using Application.Interfaces;
using Domain;
using Infrastructure.Interfaces;

namespace Application;

public class ItemService : IItemService
{
    private readonly IRepository<Item> _itemRepository;
    public ItemService(IRepository<Item> itemRepository)
    {
        _itemRepository = itemRepository;
    }
    public List<Item> GetItemsByList()
    {
        throw new NotImplementedException();
    }

    public Item AddItemToList(Item item)
    {
        return _itemRepository.Create(item);
    }

    public Item RemoveItemFromList(Item item)
    {
        throw new NotImplementedException();
    }

    public Item UpdateItemInList(Item item)
    {
        throw new NotImplementedException();
    }
}