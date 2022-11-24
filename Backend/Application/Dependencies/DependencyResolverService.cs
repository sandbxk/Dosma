using Application.Interfaces;
using Backend.Application;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Dependencies;

public class DependencyResolverService
{
    public static void RegisterApplicationLayer(IServiceCollection services)
    {
        services.AddScoped<IGroceryListService, GroceryListService>();
        services.AddScoped<IItemService, ItemService>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
    }
}