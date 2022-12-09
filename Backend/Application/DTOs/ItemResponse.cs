using Domain;

namespace Application.DTOs;

public class ItemResponse
{
    public int Id {get;set;}
    public string Title { get; set; }
    public int Quantity { get; set; }
    
    public ListItemStatus Status { get; set; }
    public ListItemCategory Category { get; set; }
}