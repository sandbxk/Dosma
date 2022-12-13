using Application.DTOs;
using Application.Interfaces;
using Application.Valdiators;
using AutoMapper;
using Domain;
using FluentValidation;
using Infrastructure.Interfaces;

namespace Application;

public class ItemService : IItemService
{
    private readonly IRepository<Item> _itemRepository;
    private readonly ItemValidators _validator;
    private readonly ItemDTOValidator _itemDTOValidator;
    private readonly IMapper _mapper;


    public ItemService(IRepository<Item> itemRepository, ItemValidators validator, ItemDTOValidator dtoValidator, IMapper mapper)
    {
        _itemRepository = itemRepository;
        _validator = validator;
        _itemDTOValidator = dtoValidator;
        _mapper = mapper;
    }

    public Item AddItem(ItemDTO itemDTO)
    {
        var validation = _itemDTOValidator.Validate(itemDTO);

        if (!validation.IsValid)
            throw new ValidationException(validation.ToString());
        
        return _itemRepository.Create(_mapper.Map<Item>(itemDTO));
    }

    public bool DeleteItem(int id, TokenUser user)
    {
        
        
        
        return _itemRepository.Delete(id);
    }

    public Item UpdateItem(Item item)
    {
        var validation = _validator.Validate(item);

        if (!validation.IsValid)
            throw new ValidationException(validation.ToString());
        
        return _itemRepository.Update(item);
    }
}