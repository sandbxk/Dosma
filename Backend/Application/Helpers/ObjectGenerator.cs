
using Application.DTOs;
using Domain;

namespace Application.Helpers;

/// <summary>
///     helper class for generating objects.
///     used for stuff that auto-mapper can't
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

    public static ItemResponse ToResponse(this Item item)
    {
        return new ItemResponse {
            Id = item.Id,
            Category = item.Category,
            Quantity = item.Quantity,
            Status = item.Status,
            Title = item.Title
        };
    }

    public static List<ItemResponse> ToResponse(this List<Item> items)
    {
        List<ItemResponse> result = new();

        foreach (var item in items)
        {
            result.Add(item.ToResponse());
        }

        return result;
    }

    public static UserResponse ToResponse(this User user)
    {
        return new UserResponse {
            Username = user.Username,
            DisplayName = user.DisplayName
        };
    }

    public static List<UserResponse> ToResponse(this List<User> users)
    {
        List<UserResponse> result = new();

        foreach (var user in users)
        {
            result.Add(user.ToResponse());
        }

        return result;
    }

    public static GroceryListResponse ToResponse(this GroceryList groceryList)
    {
        return new GroceryListResponse {
            Id = groceryList.Id,
            Items = groceryList.Items.ToResponse(),
            Title = groceryList.Title,
            Users = groceryList.Users.ToResponse()
        };
    }

    public static List<GroceryListResponse> ToResponse(this List<GroceryList> groceryLists)
    {
       List<GroceryListResponse> result = new();

        foreach (var groceryList in groceryLists)
        {
            result.Add(groceryList.ToResponse());
        }

        return result;
    }
}
