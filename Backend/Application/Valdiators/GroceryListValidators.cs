using System.Data;
using Application.DTOs;
using Application.DTOs.Response;
using Domain;
using FluentValidation;

namespace Application.Valdiators;

public class GroceryListValidator : AbstractValidator<GroceryList>
{
    public GroceryListValidator()
    {
        RuleFor(x=>x.Title).NotEmpty().WithMessage("Title is required"); 
    }
}

public class PostGroceryListValidator : AbstractValidator<GroceryListResponse>
{
    public PostGroceryListValidator()
    {
        RuleFor(x => x.Title).NotEmpty().WithMessage("Title is required");
    }
}