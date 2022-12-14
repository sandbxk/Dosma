using Domain;

namespace Application.DTOs.Requests;

public class ItemRequest
{
    public string Title { get; set; }
    public int Quantity { get; set; }
    
    public ListItemStatus Status { get; set; }
    public String Category { get; set; }
    
    public int GroceryListId { get; set; }
}

public class ItemUpdateRequest : ItemRequest
{
    public int Id { get; set; }
}