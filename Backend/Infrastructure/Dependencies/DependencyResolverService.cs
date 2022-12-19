using Domain;
using Infrastructure.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Dependencies;

public class DependencyResolverService
{
    public static void RegisterInfrastructureLayer(IServiceCollection services)
    {
        services.AddScoped<IDatabase, DatabaseRepository>();
        services.AddScoped<IRepository<GroceryList>, GroceryListRepository>();
        services.AddScoped<IRepository<Item>, ItemRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserGroceryBinding, UserGroceryRepository>();
    }
}