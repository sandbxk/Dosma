namespace Infrastructure;

using Domain;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Collections.Generic;

public class UserGroceryRepository : IUserGroceryBinding
{
    private DatabaseContext _dbContext;

    public UserGroceryRepository(DatabaseContext context)
    {
        _dbContext = context;
    }

   
    public bool AddUserToGroceryList(int userId, int groceryListId)
    {
        // check if user and grocery list exists
        var user = _dbContext.UserTable.Find(userId) ?? throw new NullReferenceException("User not found.");
        var groceryList = _dbContext.GroceryListsTable.Find(groceryListId) ?? throw new NullReferenceException("Grocery list not found.");

        // check if user is already in grocery list
        if (_dbContext.GroceryListUserJoinTable.Any(ug => ug.UserID == userId && ug.GroceryListID == groceryListId))
        {
            throw new InvalidOperationException("User is already in grocery list.");
        }

        // add binding
        _dbContext.GroceryListUserJoinTable.Add(new UserList { UserID = userId, GroceryListID = groceryListId });
        
        // save changes
        int change = _dbContext.SaveChanges();

        return change > 0;
    }

    public List<GroceryList> GetAllGroceryLists(int userId)
    {
        var lists = _dbContext.GroceryListUserJoinTable.Where(ug => ug.UserID == userId).Select(x => x.GroceryList).ToList();

        foreach (var groceryList in lists)
        {
            groceryList.Items = _dbContext.ItemTable.Where(i => i.GroceryListId == groceryList.Id).ToList();
            groceryList.Users = _dbContext.GroceryListUserJoinTable.Where(u => u.GroceryListID == groceryList.Id).Select(u => u.User).ToList();
        }

        return lists;
    }

    public bool IsUserInGroceryList(int userId, int groceryListId)
    {
        return _dbContext.GroceryListUserJoinTable.Any(u => u.UserID == userId && u.GroceryListID == groceryListId);
    }

    public List<User> GetAllUsers(int groceryListId)
    {
        return _dbContext.GroceryListUserJoinTable.Where(ug => ug.GroceryListID == groceryListId).Select(x => x.User).ToList();
    }

    public bool RemoveUserFromGroceryList(int userId, int groceryListId)
    {
        // check if binding exists
        if (_dbContext.GroceryListUserJoinTable.Any(ug => ug.UserID == userId && ug.GroceryListID == groceryListId))
        {
            // check if user and grocery list exists
            var user = _dbContext.UserTable.Find(userId) ?? throw new NullReferenceException("User not found.");
            var groceryList = _dbContext.GroceryListsTable.Find(groceryListId) ?? throw new NullReferenceException("Grocery list not found.");

            // remove binding
            _dbContext.GroceryListUserJoinTable.Remove(new UserList { UserID = userId, GroceryListID = groceryListId });

            // save changes
            int change = _dbContext.SaveChanges();
            
            return change > 0;
        }

        throw new InvalidOperationException("Binding does not exist.");
    }
}

