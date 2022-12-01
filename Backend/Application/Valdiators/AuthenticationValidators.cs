using Application.DTOs;
using Domain;
using FluentValidation;

namespace Application.Validators;

public class LoginValidator : AbstractValidator<LoginRequest>
{
    public LoginValidator()
    {
        RuleFor(x => x.Username).NotNull().NotEmpty().WithMessage("Username cannot be empty.");
        RuleFor(x => x.Username).MinimumLength(3).WithMessage("Username must be at least 3 characters long");
        RuleFor(x => x.Username).MaximumLength(20).WithMessage("Username must be at most 20 characters long");
        RuleFor(x => x.Username).Matches("^[a-zA-Z0-9]*$").WithMessage("Username must only contain alphanumeric characters");

        RuleFor(x => x.Password).NotNull().NotEmpty().WithMessage("Password cannot be empty.");
    }
}

public class RegisterValidator : AbstractValidator<RegisterRequest>
{
    public RegisterValidator()
    {
        RuleFor(x => x.Username).NotNull().NotEmpty().WithMessage("Username cannot be empty.");
        RuleFor(x => x.Username).MinimumLength(3).WithMessage("Username must be at least 3 characters long");
        RuleFor(x => x.Username).MaximumLength(20).WithMessage("Username must be at most 20 characters long");
        RuleFor(x => x.Username).Matches("^[a-zA-Z0-9]*$").WithMessage("Username must only contain alphanumeric characters");

        RuleFor(x => x.Password).NotNull().NotEmpty().WithMessage("Password cannot be empty.");
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