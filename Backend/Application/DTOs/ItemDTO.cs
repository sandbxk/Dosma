using Domain;

namespace Application.DTOs;

public class ItemDTO
{
    public string Title { get; set; }
    public int Quantity { get; set; }
    
    public ListItemStatus Status { get; set; }
    public ListItemCategory Category { get; set; }
    public int GroceryListId { get; set; }
}