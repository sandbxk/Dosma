using System.Collections;
using Application.DTOs;
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
    private IValidator<GroceryList> _validator;
    
    public GroceryListService(IRepository<GroceryList> repository, IMapper mapper, IValidator<GroceryListDTO> dtoValidator, IValidator<GroceryList> validator)
    {
        _groceryListRepository = repository;
        _mapper = mapper;
        _dtoValidator = dtoValidator;
        _validator = validator;
    }


    public GroceryList Create(GroceryListDTO dto)
    {
        var validation = _dtoValidator.Validate(dto);
        if (!validation.IsValid)
            throw new ValidationException(validation.ToString());
        
        return _groceryListRepository.Create(_mapper.Map<GroceryList>(dto));
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