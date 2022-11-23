namespace Domain;

public class GroceryList
{
    public int Id { get; set; }
    public string Title { get; set; }
    public IEnumerable<User> UserAccess { get; set; } // this is the list of users that have access to this list
    public IEnumerable<Item> Items { get; set; }
    public DateTime Created { get; set; }
    public DateTime Modified { get; set; }
    public bool IsArchived { get; set; }
}