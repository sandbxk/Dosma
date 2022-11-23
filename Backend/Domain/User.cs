namespace Domain;

public class User
{
    public int Id { get; set; }
    public string DisplayName { get; set;}

    public ICollection<UserList> GroceryLists { get; set; }
    
    //TODO JWT
}