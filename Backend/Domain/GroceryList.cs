namespace Domain;

public class GroceryList
{
    public int Id { get; set; } 
    public string Title { get; set; } = string.Empty;
    
    public List<Item> Items { get; set; } = new List<Item>();
    
    public ICollection<UserList> Users { get; set; } = new List<UserList>();
}