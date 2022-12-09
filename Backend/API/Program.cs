using System.Text;
using System.Text.Json.Serialization;
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

builder.Services.AddControllers().AddJsonOptions(x =>
    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddDbContext<DatabaseContext>(options =>
{
    options.UseSqlite(
        "Data Source=db.db"
    );
    options.EnableSensitiveDataLogging();
    options.EnableDetailedErrors();
});

ApplicationDependencies.RegisterApplicationLayer(builder.Services);
ApplicationDependencies.RegisterSecurityLayer(builder.Services, Encoding.ASCII.GetBytes("This is a secret"));

InfrastructureDependencies.RegisterInfrastructureLayer(builder.Services);

var mapper = new MapperConfiguration(config => {
    config.CreateMap<GroceryListDTO, GroceryList>();
    config.CreateMap<ItemDTO, Item>();
    config.CreateMap<TokenUserDTO, User>();
    config.CreateMap<User, TokenUserDTO>();

    //config.CreateMap<GroceryList, GroceryListResponse>();
    //config.CreateMap<List<GroceryList>, List<GroceryListResponse>>();
    //
    //config.CreateMap<Item, ItemResponse>();
    //config.CreateMap<List<Item>, List<ItemResponse>>();
    //
    //config.CreateMap<User, UserResponse>();
    //config.CreateMap<List<User>, List<UserResponse>>();
}).CreateMapper();


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

app.UseCors(options =>
{
    options.SetIsOriginAllowed(origin => true)
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials();
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
