namespace Domain;

public class UserList
{
    public int UserID { get; set; }
    public User User { get; set; }
    
    public int GroceryListID { get; set; }
    public GroceryList GroceryList { get; set; }
}