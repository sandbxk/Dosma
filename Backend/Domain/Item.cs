namespace Domain;

public class Item
{
    public int Id { get; set; }
    public string Title { get; set; }
    public int Quantity { get; set; }
    
    public ListItemStatus Status { get; set; }
    public ListItemCategory Category { get; set; }
    
    public int GroceryListId { get; set; }
    //TODO: Expected cost
} 
