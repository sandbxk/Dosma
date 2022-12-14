
using Application.DTOs;
using Application.DTOs.Requests;
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

    public static TokenUser ToTokenUser(this User user)
    {
        return new TokenUser
        {
            Id = user.Id,
            Username = user.Username,
            DisplayName = user.DisplayName
        };
    }

    public static UserResponse UserToDTO(this User user)
    {
        return new UserResponse
        {
            Id = user.Id,
            Username = user.Username,
            DisplayName = user.DisplayName
        };
    }

    public static List<UserResponse> UsersToDTO(this List<User> users)
    {
        List<UserResponse> userDTOs = new();

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

    public static List<GroceryListResponse> GroceryListsToResponses(this List<GroceryList> lists)
    {
        List<GroceryListResponse> userDTOs = new();

        foreach (var list in lists)
        {
            userDTOs.Add(list.GroceryListToResponse());
        }
        return userDTOs;
    }

    public static GroceryList RequestToGrocerylist(this GroceryListUpdateRequest _this)
    {
        return new GroceryList()
        {
            Id = _this.Id,
            Title = _this.Title,
            Items = new List<Item>(),
            Users = new List<User>(),
            SharedList = new List<UserList>()
        };
    }

    public static GroceryList RequestToGrocerylist(this GroceryListCreateRequest _this)
    {
        return new GroceryList()
        {
            Title = _this.Title,
            Items = new List<Item>(),
            Users = new List<User>(),
            SharedList = new List<UserList>()
        };
    }
    
    public static Item RequestToItem(this ItemCreateRequest _this)
    {
        return new Item()
        {
            Title = _this.Title,
            Amount = _this.Amount,
        };
    }
}
