using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Backend.Application;
using Domain;
using FluentValidation;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Application.Dependencies;

public class DependencyResolverService
{
    public static void RegisterApplicationLayer(IServiceCollection services)
    {
        services.AddScoped<IGroceryListService, GroceryListService>();
        services.AddScoped<IItemService, ItemService>();
    }

    /*
        manually add all the dependencies here

        reason: serverSecrets is needed for the JWT token generation and in the startup sequence. 
                to store it in one place, reducing the risk of breaking authentication.
    */
    public static void RegisterSecurityLayer(IServiceCollection services, byte[] serverSecret)
    {
        services.AddScoped<IAuthenticationService, AuthenticationService>(x => new AuthenticationService(
            x.GetService<IUserRepository>() ?? throw new Exception("User repository not found"),
            x.GetService<IMapper>() ?? throw new Exception("Mapper not found"),
            x.GetService<IValidator<LoginRequestDTO>>() ?? throw new Exception("Login validator not found"),
            x.GetService<IValidator<User>>() ?? throw new Exception("User validator not found"),
            serverSecret
        ));

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(serverSecret),
                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromMinutes(5)
            };
        });
    }
}