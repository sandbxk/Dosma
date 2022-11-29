


using Moq;
using Application.Interfaces;
using Application;
using Backend.Application;
using Application.DTOs;
using Infrastructure.Interfaces;
using Domain;
using Backend.Application.Helpers;
using FluentValidation;
using AutoMapper;
using System.Text;
using Application.Validators;
using FluentValidation.Results;

namespace Backend.Tests;

public class AuthenticationTests
{
    private IAuthenticationService GetMockAuthenticationService(List<User> db_users)
    {
        Mock<IUserRepository> user_repo = new Mock<IUserRepository>();
        user_repo.Setup(x => x.Find(It.IsAny<string>())).Returns((string username) => db_users.FirstOrDefault(x => x.Username == username) ?? throw new Exception("User not found"));
        user_repo.Setup(x => x.Create(It.IsAny<User>())).Callback((User user) => db_users.Add(user));
        user_repo.Setup(x => x.Update(It.IsAny<User>())).Callback((User user) => db_users[db_users.FindIndex(x => x.Username == user.Username)] = user);
        user_repo.Setup(x => x.Delete(It.IsAny<int>())).Callback((int id) => db_users.RemoveAt(db_users.FindIndex(x => x.Id == id)));
        user_repo.Setup(x => x.All()).Returns(db_users);
        user_repo.Setup(x => x.Single(It.IsAny<int>())).Returns((int id) => db_users.FirstOrDefault(x => x.Id == id) ?? throw new Exception("User not found"));

        var mapper = new MapperConfiguration(config => {
        }).CreateMapper();;
        
        return new AuthenticationService(user_repo.Object, mapper, new LoginValidator(), new UserValidator(), Encoding.ASCII.GetBytes("This is not a secret"));
    }
    
    List<User> existing_user_database = new List<User>();

    public AuthenticationTests()
    {
       existing_user_database = new List<User>() {
         ObjectGenerator.GenerateUser(new RegisterRequestDTO {
            Username = "user1",
            DisplayName = "User 1",
            Password = "user1",
         }),
         ObjectGenerator.GenerateUser(new RegisterRequestDTO {
            Username = "user2",
            DisplayName = "user 2",
            Password = "user2",
         }),
         ObjectGenerator.GenerateUser(new RegisterRequestDTO {
            Username = "user3",
            DisplayName = "user 3",
            Password = "user2",
         }),
       };
    }

    [Theory]
    [InlineData("user1", "user1", true)]
    [InlineData("user2", "user2", true)]
    [InlineData("user3", "user2", true)]
    [InlineData("user999", "does_not_matter", false)] // User does not exist
    [InlineData("user1", "user2", false)] // Wrong password
    public void ValidateLogin(string username, string password, bool expected)
    {
        // Arrange
        IAuthenticationService service = GetMockAuthenticationService(existing_user_database);

        var loginData = new LoginRequestDTO {
            Username = username,
            Password = password,
        };

        // Act
        bool actual = service.Login(loginData, out string token);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void RegisterNewUser()
    {
        // Arrange
        IAuthenticationService service = GetMockAuthenticationService(existing_user_database);

        var registerData = new RegisterRequestDTO {
            DisplayName = "John Doe",
            Username = "user100",
            Password = "user100",
        };

        // Act
        bool actual = service.Register(registerData, out _);

        // Assert
        Assert.Equal(true, actual);
    }

}