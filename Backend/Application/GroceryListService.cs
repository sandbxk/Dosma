using Application.DTOs;
using Application.Interfaces;
using Application.Valdiators;
using AutoMapper;
using Domain;
using FluentValidation;
using Infrastructure.Interfaces;

namespace Application;

public class GroceryListService : IGroceryListService
{
    private IRepository<GroceryList> _groceryListRepository;
    private IMapper _mapper;
    private IValidator<GroceryListValidators> _Validator;
    private IValidator<GroceryListDTO> _DTOValidator;
    
    public GroceryListService(IRepository<GroceryList> repository, IMapper mapper, IValidator<GroceryListValidators> postValidator, IValidator<GroceryListDTO> DTOValidator)
    {
        _groceryListRepository = repository;
        _mapper = mapper;
        _DTOValidator = DTOValidator;
        _Validator = postValidator;
    }


    public GroceryList Create(GroceryListDTO dto)
    {
        var validation = _DTOValidator.Validate(dto);

        if (!validation.IsValid)
            throw new ValidationException(validation.ToString());
        
        return _groceryListRepository.Create(_mapper.Map<GroceryList>(dto));
    }
}