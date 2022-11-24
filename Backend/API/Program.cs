using Application.DTOs;
using AutoMapper;
using Domain;
using FluentValidation;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

using ApplicationDependencies = Application.Dependencies.DependencyResolverService;
using InfrastructureDependencies = Infrastructure.Dependencies.DependencyResolverService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddDbContext<DatabaseContext>(options => options.UseSqlite(
    "Data Source=db.db"
));

ApplicationDependencies.RegisterApplicationLayer(builder.Services);
InfrastructureDependencies.RegisterInfrastructureLayer(builder.Services);

var mapper = new MapperConfiguration(config =>
{
    config.CreateMap<GroceryListDTO, GroceryList>();
    config.CreateMap<ItemDTO, Item>();
}).CreateMapper();

Application.Dependencies
    .DependencyResolverService
    .RegisterApplicationLayer(builder.Services);

Infrastructure.Dependencies
    .DependencyResolverService
    .RegisterInfrastructureLayer(builder.Services);

builder.Services.AddSingleton(mapper);

builder.Services.AddCors();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();