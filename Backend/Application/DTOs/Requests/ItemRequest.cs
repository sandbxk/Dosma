using Domain;

namespace Application.DTOs.Requests;

public class ItemRequest
{
    public string Title { get; set; }
    public int Quantity { get; set; }
    
    public ListItemStatus Status { get; set; }
    public ListItemCategory Category { get; set; }
    
    public int GroceryListId { get; set; }
}