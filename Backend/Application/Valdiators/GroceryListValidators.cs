using Application.DTOs;
using Domain;
using FluentValidation;

namespace Application.Valdiators;

public class GroceryListValidators : AbstractValidator<GroceryList>
{
    public GroceryListValidators()
    {
        RuleFor(x=>x.Title).NotEmpty().WithMessage("Title is required");
    }
}

public class PostBoxValidator : AbstractValidator<GroceryListDTO>
{
    public PostBoxValidator()
    {
        RuleFor(x => x.Title).NotEmpty().WithMessage("Title is required");
    }
}