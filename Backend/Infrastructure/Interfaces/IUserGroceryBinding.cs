namespace Infrastructure.Interfaces;

using Domain;

public interface IUserGroceryBinding
{
    bool AddUserToGroceryList(int userId, int groceryListId);
    bool RemoveUserFromGroceryList(int userId, int groceryListId);

    List<User> GetAllUsers(int groceryListId);

    List<GroceryList> GetAllGroceryLists(int userId);
    
    bool IsUserInGroceryList(int userId, int groceryListId);
}

