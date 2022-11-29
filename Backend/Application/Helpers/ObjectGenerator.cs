
using Application.DTOs;
using Application.Helpers;
using Domain;

namespace Backend.Application.Helpers;

public static class ObjectGenerator
{
    public static User GenerateUser(RegisterRequestDTO registration)
    {
        (var salt, var hash) = HashGenerator.Generate(registration.Password);

        return new User
        {
            Username = registration.Username,
            DisplayName = registration.DisplayName,
            HashedPassword = hash,
            Salt = salt
        };
    }
   
}