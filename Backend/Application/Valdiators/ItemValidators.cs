using Application.DTOs;
using Domain;
using FluentValidation;

namespace Application.Valdiators;

public class ItemValidators : AbstractValidator<Item>
{
    public ItemValidators()
    {
        RuleFor(x=>x.Title).NotEmpty().WithMessage("Title is required.");
        RuleFor(x => x.GroceryListId).GreaterThanOrEqualTo(1).WithMessage("Grocery List ID must be greater than or equal to 0.");
        RuleFor(x => x.Quantity).GreaterThanOrEqualTo(1).WithMessage("Quantity must be greater than or equal to 1.");
        RuleFor(x => x.Status).IsInEnum().WithMessage("Status must be a valid status.");
        RuleFor(x => x.Category).IsInEnum().WithMessage("Category must be a valid category.");
    }
}