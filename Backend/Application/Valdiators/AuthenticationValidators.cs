using Application.DTOs;
using Domain;
using FluentValidation;

namespace Application.Validators;

public class LoginValidator : AbstractValidator<LoginRequestDTO>
{
    public LoginValidator()
    {
        RuleFor(x => x.Username).NotEmpty().WithMessage("Username is required");
        RuleFor(x => x.Username).MinimumLength(3).WithMessage("Username must be at least 3 characters long");
        RuleFor(x => x.Username).MaximumLength(20).WithMessage("Username must be at most 20 characters long");

        RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required");
    }
}

public class UserValidator : AbstractValidator<User>
{
    public UserValidator()
    {
        RuleFor(x => x.HashedPassword).NotEmpty().WithMessage("Password is required");
        RuleFor(x => x.Salt).NotEmpty().WithMessage("Salt is required");

        RuleFor(x => x.Username).NotEmpty().WithMessage("Username is required");
        RuleFor(x => x.Username).MinimumLength(3).WithMessage("Username must be at least 3 characters long");
        RuleFor(x => x.Username).MaximumLength(20).WithMessage("Username must be at most 20 characters long");
    }
}