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
    private IValidator<GroceryListResponse> _dtoResponseValidator;
    private readonly IValidator<GroceryListRequest> _dtoRequestValidator;
    private IValidator<GroceryList> _validator;
    
    public GroceryListService(IRepository<GroceryList> repository, IMapper mapper, IValidator<GroceryListResponse> dtoResponseValidator, IValidator<GroceryListRequest> dtoRequestValidator, IValidator<GroceryList> validator)
    {
        _groceryListRepository = repository;
        _mapper = mapper;
        _dtoResponseValidator = dtoResponseValidator;
        _dtoRequestValidator = dtoRequestValidator;
        _validator = validator;
    }


    public GroceryListResponse Create(GroceryListRequest request)
    {
        var validation = _dtoRequestValidator.Validate(request);
        if (!validation.IsValid)
            throw new ValidationException(validation.ToString());
        
        var groceryList =  _groceryListRepository.Create(request.requestToGrocerylist());
        return groceryList.GroceryListToResponse();
    }

    public GroceryListResponse GetListById(int id)
    {
        var grocerylist = _groceryListRepository.Single(id);

        if (grocerylist == null)
            throw new ValidationException("Grocery list not found");

        return grocerylist.GroceryListToResponse();
    }

    public List<GroceryList> GetListsByUser(User user)
    {
        return _groceryListRepository.All();
    }
    
    #if DEBUG
    public List<GroceryList> GetAllLists()
    {
        return _groceryListRepository.All();
    }
    #endif

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