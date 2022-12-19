using Domain;

namespace Application.DTOs.Response;

public class ItemResponse
{
    public int Id { get; set; }
    public string Title { get; set; }
    public int Quantity { get; set; }
    
    public ListItemStatus Status { get; set; }
    public String Category { get; set; }
    
    public int GroceryListId { get; set; }
}