namespace Domain;

public class UserList
{
    public int UserID { get; set; }
    public User User { get; set; } = null!;
    public int GroceryListID { get; set; }
    public GroceryList GroceryList { get; set; } = null!;
    public AccessLevel AccessLevel { get; set; } = AccessLevel.NONE;
    public AccessState AccessState { get; set; } = AccessState.NONE;
}
