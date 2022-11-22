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
    private IValidator<GroceryListValidators> _postValidator;
    private IValidator<GroceryListDTO> _postDTOValidator;
    
    public GroceryListService(IRepository<GroceryList> repository, IMapper mapper, IValidator<GroceryListValidators> postValidator, IValidator<GroceryListDTO> postDTOValidator)
    {
        _groceryListRepository = repository;
        _mapper = mapper;
        _postDTOValidator = postDTOValidator;
        _postValidator = postValidator;
    }


    public GroceryList Create(GroceryListDTO dto)
    {
        return _groceryListRepository.Create(dto);
    }
}