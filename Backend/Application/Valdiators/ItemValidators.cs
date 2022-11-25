using Application.DTOs;
using Domain;
using FluentValidation;

namespace Application.Valdiators;

public class ItemValidators : AbstractValidator<Item>
{
    public ItemValidators()
    {
        RuleFor(x=>x.Title).NotEmpty().WithMessage("Title is required");
        RuleFor(x => x.GroceryListId).GreaterThan(0).WithMessage("Grocery List ID is must be greater than 0");
    }
}

public class ItemDTOValidator : AbstractValidator<ItemDTO>
{
    public ItemDTOValidator()
    {
        RuleFor(x => x.Title).NotEmpty().WithMessage("Title is required");
        RuleFor(x => x.GroceryListId).GreaterThan(0).WithMessage("Grocery List ID is must be greater than 0");
    }
}