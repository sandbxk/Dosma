using Application.DTOs;
using Application.Interfaces;
using Application.Valdiators;
using Domain;
using FluentValidation;
using Infrastructure.Interfaces;

namespace Application;

public class ItemService : IItemService
{
    private readonly IRepository<Item> _itemRepository;
    private readonly ItemValidators _itemValidator;
    private readonly IUserGroceryBinding _userGroceryBinding;


    public ItemService(IRepository<Item> itemRepository, ItemValidators itemItemValidator, IUserGroceryBinding userGroceryBinding)
    {
        _itemRepository = itemRepository;
        _itemValidator = itemItemValidator;
        _userGroceryBinding = userGroceryBinding;
    }

    public Item AddItem(Item item)
    {
        var validation = _itemValidator.Validate(item);

        if (!validation.IsValid)
            throw new ValidationException(validation.ToString());
        
        return _itemRepository.Create(item);
    }

    public bool DeleteItem(int id, TokenUser user)
    {
        if (_userGroceryBinding.IsUserInGroceryList(user.Id, id))
        {
            return _itemRepository.Delete(id);
        }
        throw new UnauthorizedAccessException("You are not authorized to delete this item");
    }

    public Item UpdateItem(Item item)
    {
        var validation = _itemValidator.Validate(item);

        if (!validation.IsValid)
            throw new ValidationException(validation.ToString());
        
        return _itemRepository.Update(item);
    }
}