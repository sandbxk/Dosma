
using Application.DTOs;
using Application.DTOs.Response;
using Domain;
using GroceryListResponse = Application.DTOs.Response.GroceryListResponse;

namespace Application.Helpers;

/// <summary>
///     helper class for generating objects
/// </summary>
public static class ObjectGenerator
{
    /// <summary>
    ///     Generates a user object
    /// </summary>
    /// <param name="request">The request object</param>
    /// <returns>The generated user object</returns>
    /// <remarks>
    ///     This method is used by
    ///     <list>
    ///         <item><see cref="AuthenticationService.Register"/></item>
    ///     </list>
    /// </remarks>
    /// <completionlist cref="(User, RegisterRequest, HashGenerator, AuthenticationService)"/>
    /// <author>
    ///     <name>Mads Mandahl-Barth</name>
    /// </author>
    public static User GenerateUser(RegisterRequest registration)
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
    
    public static UserDTO UserToDTO(this User user)
    {
        return new UserDTO
        {
            Id = user.Id,
            Username = user.Username,
            DisplayName = user.DisplayName
        };
    }

    public static List<UserDTO> UsersToDTO(this List<User> users)
    {
        List<UserDTO> userDTOs = new();

        foreach (var user in users)
        {
            userDTOs.Add(user.UserToDTO());
        }
        return userDTOs;
    }
    
    public static GroceryListResponse GroceryListToResponse(this GroceryList groceryList)
    {
            return new GroceryListResponse
        {
            Id = groceryList.Id,
            Title = groceryList.Title,
            Items = groceryList.Items,
            Users = groceryList.Users.UsersToDTO()
        };
    }

    public static GroceryList requestToGrocerylist(this GroceryListRequest response)
    {
        return new GroceryList()
        {
            Title = response.Title,
            Items = new List<Item>(),
            Users = new List<User>(),
            SharedList = new List<UserList>()
        };
    }
}
