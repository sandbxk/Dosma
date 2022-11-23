using Domain;
using Infrastructure.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Dependencies;

public class DependencyResolverService
{
    public static void RegisterInfrastructureLayer(IServiceCollection services)
    {
        services.AddScoped<IDatabase, DatabaseRepistory>();
        services.AddScoped<IRepository<GroceryList>, GroceryListRepository>();
    }
}