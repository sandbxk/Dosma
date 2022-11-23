namespace Domain;

public class GroceryList
{
    public int Id { get; set; }
    public string Title { get; set; }
    public ICollection<UserList> Users { get; set; }
    public ICollection<Item> Items { get; set; }
}