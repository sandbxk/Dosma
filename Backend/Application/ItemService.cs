﻿using Application.DTOs;
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
    private ItemValidators _validator;
    private ItemDTOValidator _itemDTOValidator;
    private IMapper _mapper;
    public ItemService(IRepository<Item> itemRepository, ItemValidators validator, ItemDTOValidator dtoValidator, IMapper mapper)
    {
        _itemRepository = itemRepository;
        _validator = validator;
        _itemDTOValidator = dtoValidator;
        _mapper = mapper;
    }
    public List<Item> GetItemsByList()
    {
        throw new NotImplementedException();
    }

    public Item AddItemToList(ItemDTO itemDTO)
    {
        var validation = _itemDTOValidator.Validate(itemDTO);

        if (!validation.IsValid)
            throw new ValidationException(validation.ToString());
        
        return _itemRepository.Create(_mapper.Map<Item>(itemDTO));
    }

    public Item DeleteItemFromList(int id, Item item)
    {
        if (id != item.Id)
            throw new ValidationException("List ID does not match ID in URL.");
        
        var validation = _validator.Validate(item);
        if (!validation.IsValid)
            throw new ValidationException(validation.ToString());

        return _itemRepository.Delete(item.Id ?? throw new Exception());
    }

    public Item UpdateItemInList(Item item)
    {
        throw new NotImplementedException();
    }
}