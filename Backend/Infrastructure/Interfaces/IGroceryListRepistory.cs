using Domain;

namespace Infrastructure.Interfaces;

public interface IGroceryListRepository : IRepository<GroceryList>
{
    public List<UserList> AddUser(User user, GroceryList groceryList);
    public List<UserList> RemoveUser(User user, GroceryList groceryList);
}