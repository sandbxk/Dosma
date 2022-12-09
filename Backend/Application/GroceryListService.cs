using System.Collections;
using Application.DTOs;
using Application.DTOs.Response;
using Application.Helpers;
using Application.Interfaces;
using AutoMapper;
using Domain;
using FluentValidation;
using Infrastructure.Interfaces;

namespace Application;

public class GroceryListService : IGroceryListService
{
    private IRepository<GroceryList> _groceryListRepository;
    private IMapper _mapper;
    private IValidator<GroceryListResponse> _dtoValidator;
    private IValidator<GroceryList> _validator;
    
    public GroceryListService(IRepository<GroceryList> repository, IMapper mapper, IValidator<GroceryListResponse> dtoValidator, IValidator<GroceryList> validator)
    {
        _groceryListRepository = repository;
        _mapper = mapper;
        _dtoValidator = dtoValidator;
        _validator = validator;
    }


    public GroceryList Create(GroceryListResponse response)
    {
        var validation = _dtoValidator.Validate(response);
        if (!validation.IsValid)
            throw new ValidationException(validation.ToString());
        
        return _groceryListRepository.Create(_mapper.Map<GroceryList>(response));
    }

    public GroceryListResponse GetListById(int id)
    {
        var grocerylist = _groceryListRepository.Single(id);
        
        foreach (var User in grocerylist.Users)
        {
            User.UserToDTO();
        }
        
        if (grocerylist == null)
            throw new ValidationException("Grocery list not found");

        return grocerylist.GroceryListToDTO();
    }

    public List<GroceryList> GetListsByUser(User user)
    {
        return _groceryListRepository.All();
    }

    public List<GroceryList> GetAllLists()
    {
        return _groceryListRepository.All();
    }

    public bool DeleteList(GroceryList groceryList)
    {
        var validation = _validator.Validate(groceryList);
        if (!validation.IsValid)
            throw new ValidationException(validation.ToString());
        
        return _groceryListRepository.Delete(groceryList.Id);
    }

    public GroceryList UpdateList(int id, GroceryList groceryList)
    {
        var validation = _validator.Validate(groceryList);
        if (!validation.IsValid)
            throw new ValidationException(validation.ToString());
        
        return _groceryListRepository.Update(groceryList);
    }
}