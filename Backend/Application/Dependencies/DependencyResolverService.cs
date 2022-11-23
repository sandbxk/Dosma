﻿using Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Dependencies;

public class DependencyResolverService
{
    public static void RegisterApplicationLayer(IServiceCollection services)
    {
        services.AddScoped<IGroceryListService, GroceryListService>();
    }
}