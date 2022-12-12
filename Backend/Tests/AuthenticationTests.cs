using Moq;
using Application;
using Application.Interfaces;
using Application.Validators;
using Application.Helpers;
using Application.DTOs;
using Infrastructure.Interfaces;
using Domain;
using AutoMapper;

using System.Text;
using Application.DTOs.Requests;

namespace Backend.Tests;

public class AuthenticationTests
{
    private IAuthenticationService GetMockAuthenticationService(List<User> db_users)
    {
        Mock<IUserRepository> user_repo = new Mock<IUserRepository>();

        user_repo.Setup(x => x.Find(It.IsAny<string>())).Returns((string username) => {
            return db_users.FirstOrDefault(x => x.Username == username) ?? throw new NullReferenceException("User not found");
        });

        user_repo.Setup(x => x.Create(It.IsAny<User>())).Returns((User user) => {
            db_users.Add(user);
            return user;
        });

        user_repo.Setup(x => x.Update(It.IsAny<User>())).Returns((User user) => {
            db_users[db_users.FindIndex(x => x.Username == user.Username)] = user;
            return user;
        });

        user_repo.Setup(x => x.Delete(It.IsAny<int>())).Returns((int id) => {
            var user = db_users.FirstOrDefault(x => x.Id == id);
            if (user == null)
                return false;
            db_users.Remove(user);
            return true;
        });
          
        user_repo.Setup(x => x.All()).Returns(db_users);
        user_repo.Setup(x => x.Single(It.IsAny<int>())).Returns((int id) => db_users.FirstOrDefault(x => x.Id == id) ?? throw new NullReferenceException("User not found"));

        var mapper = new MapperConfiguration(config => {}).CreateMapper();;
        
        return new AuthenticationService(
            user_repo.Object, 
            mapper, 
            new LoginValidator(),
            new RegisterValidator(), 
            new UserValidator(), 
            Encoding.ASCII.GetBytes("This is not a secret")
        );
    }
    
    List<User> existing_user_database = new List<User>();

    public AuthenticationTests()
    {
       existing_user_database = new List<User>() {
         ObjectGenerator.GenerateUser(new RegisterRequest {
            Username = "user1",
            DisplayName = "User 1",
            Password = "user1",
         }),
         ObjectGenerator.GenerateUser(new RegisterRequest {
            Username = "user2",
            DisplayName = "user 2",
            Password = "user2",
         }),
         ObjectGenerator.GenerateUser(new RegisterRequest {
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

        var loginData = new LoginRequest {
            Username = username,
            Password = password,
        };

        // Act
        bool actual = service.Login(loginData, out string token);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("New user", "user100", "user100", true)]
    [InlineData("Already Exists", "user1", "user1", false)]
    [InlineData("Invalid User", "user101", "", false)]        
    public void ValidateRegister(string dn, string un, string pw, bool expected)
    {
        // Arrange
        IAuthenticationService service = GetMockAuthenticationService(existing_user_database);

        var registerData = new RegisterRequest {
            DisplayName = dn,
            Username = un,
            Password = pw,
        };

        // Act
        bool actual = service.Register(registerData, out string token);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void RegisterUserNoDisplayName()
    {
        // Arrange
        IAuthenticationService service = GetMockAuthenticationService(existing_user_database);

        var registerData = new RegisterRequest {
            Username = "user102",
            Password = "user102",
        };

        // Act
        bool actual = service.Register(registerData, out _);

        // Assert
        Assert.Equal(true, actual); // No display name should be allowed

        // Check that the display name is the same as the username
        Assert.Equal(registerData.Username, existing_user_database.Find(x => x.Username == registerData.Username)?.DisplayName);
    }
}