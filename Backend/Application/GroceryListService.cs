using System.Collections;
using System.ComponentModel.DataAnnotations;
using Application.DTOs;
using Application.DTOs.Response;
using Application.Helpers;
using Application.Interfaces;
using AutoMapper;
using Domain;
using FluentValidation;
using Infrastructure;
using Infrastructure.Interfaces;

namespace Application;

public class GroceryListService : IGroceryListService
{
    private readonly IRepository<GroceryList> _groceryListRepository;
    private readonly IUserGroceryBinding _userGroceryRepository;

    private readonly IValidator<GroceryListResponse> _groceryListResponseValidator;
    private readonly IValidator<GroceryListCreateRequest> _groceryListCreateRequestValidator;
    private readonly IValidator<GroceryListUpdateRequest> _groceryListUpdateRequestValidator;
    private readonly IValidator<GroceryList> _groceryListValidator;
    
    public GroceryListService(
        IRepository<GroceryList> repository,
        IUserGroceryBinding userGroceryRepository,
        
        IValidator<GroceryListResponse> responseValidator, 
        IValidator<GroceryListCreateRequest> createRequestValidator,
        IValidator<GroceryListUpdateRequest> updateRequestValidator,
        IValidator<GroceryList> validator
    )
    {
        _groceryListRepository = repository;
        _userGroceryRepository = userGroceryRepository;

        _groceryListResponseValidator = responseValidator;
        _groceryListCreateRequestValidator = createRequestValidator;
        _groceryListUpdateRequestValidator = updateRequestValidator;
        _groceryListValidator = validator;
    }

    public GroceryListResponse Create(GroceryListCreateRequest request, TokenUser user)
    {
        // Validate the request
        _groceryListCreateRequestValidator.ValidateAndThrow(request);

        // Map the request to a GroceryList
        var mapped = request.RequestToGrocerylist();

        // Create and validate
        var groceryList = _groceryListRepository.Create(mapped);
        _groceryListValidator.ValidateAndThrow(groceryList);

        // map to a response and validate it
        var response = groceryList.GroceryListToResponse();
        _groceryListResponseValidator.ValidateAndThrow(response);

        // Bind the user to the grocery list
        if (_userGroceryRepository.AddUserToGroceryList(user.Id, groceryList.Id))
        {
            return response;
        }

        throw new Exception("Could not bind user to grocery list");
    }

    public GroceryListResponse GetListById(int listID)
    {
        // get and validate the list
        var grocerylist = _groceryListRepository.Single(listID);
        _groceryListValidator.ValidateAndThrow(grocerylist);

        return grocerylist.GroceryListToResponse();
    }

    public List<GroceryListResponse> GetListsByUser(TokenUser user)
    {
        return _userGroceryRepository.GetAllGroceryLists(user.Id).GroceryListsToResponses();
    }
    
    public List<GroceryListResponse> GetAllLists()
    {
#if DEBUG
        return _groceryListRepository.All().GroceryListsToResponses();
#else
        return new List<GroceryListResponse>();
#endif
    }

    public bool DeleteList(int listID, TokenUser user)
    {
        // remove the user from the list
        if (_userGroceryRepository.RemoveUserFromGroceryList(user.Id, listID))
        {
            // delete list only if no other users are assigned to it.
            // without sharing this should always be true
            if (_userGroceryRepository.GetAllUsers(listID).Count() == 0)
            {
                return _groceryListRepository.Delete(listID);
            }
        }

        return false;
    }

    public GroceryListResponse UpdateList(GroceryListUpdateRequest request)
    {
        // Validate the request
        _groceryListUpdateRequestValidator.ValidateAndThrow(request);

        // Map the request to a GroceryList
        var mapped = request.RequestToGrocerylist();

        // Update and validate
        var groceryList = _groceryListRepository.Update(mapped);
        _groceryListValidator.ValidateAndThrow(groceryList);

        // map to a response and validate it
        var response = groceryList.GroceryListToResponse();
        _groceryListResponseValidator.ValidateAndThrow(response);

        return response;
    }
}