using System.Collections;
using Application.DTOs;
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
    private IValidator<GroceryListDTO> _dtoValidator;
    private IValidator<GroceryListCreateRequest> _reqValidator;
    private IValidator<GroceryList> _validator;
    
    public GroceryListService(IRepository<GroceryList> repository, IMapper mapper, IValidator<GroceryListCreateRequest> dtoValidator, IValidator<GroceryList> validator)
    {
        _groceryListRepository = repository;
        _mapper = mapper;
        _reqValidator = dtoValidator;
        _validator = validator;
    }

    public GroceryListResponse Create(GroceryListCreateRequest dto)
    {
        // var validation = _dtoValidator.Validate(dto);

        //if (!validation.IsValid)
        //    throw new ValidationException(validation.ToString());
        
        return _groceryListRepository.Create(_mapper.Map<GroceryList>(dto)).ToResponse();
    }

    public GroceryListResponse GetListById(int id)
    {
        return _groceryListRepository.Single(id).ToResponse();
    }

    public List<GroceryListResponse> GetListsByUser(User user)
    {
        return _groceryListRepository.All().ToResponse();
    }

    public List<GroceryListResponse> GetAllLists()
    {
        return _groceryListRepository.All().ToResponse();
    }

    public bool DeleteList(int id)
    {
        //var validation = _validator.Validate(groceryList);
        //if (!validation.IsValid)
        //    throw new ValidationException(validation.ToString());
        
        return _groceryListRepository.Delete(id);
    }

    public GroceryListResponse UpdateList(int id, GroceryList groceryList)
    {
        var validation = _validator.Validate(groceryList);
        if (!validation.IsValid)
            throw new ValidationException(validation.ToString());

        return _groceryListRepository.Update(groceryList).ToResponse();
    }
}