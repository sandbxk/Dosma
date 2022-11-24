namespace Domain;

public class User
{
    public int Id { get; set; }
    public string DisplayName { get; set;} = string.Empty;

    public ICollection<UserList> GroceryLists { get; set; } = new List<UserList>();
    
     //TODO JWT
    public string Username { get; set; } = string.Empty;
    public string HashedPassword { get; set; } = string.Empty;
}