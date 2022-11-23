namespace Domain;

public class GroceryList
{
    public int Id { get; set; }
    public string Title { get; set; }
    
    public IEnumerable<Item> Items { get; set; }
}