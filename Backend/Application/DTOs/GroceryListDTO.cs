using Domain;

namespace Application.DTOs;

public class GroceryListDTO
{
    public string Title { get; set; }
    public IEnumerable<User> UserAccess { get; set; } // this is the list of users that have access to this list
    public IEnumerable<ListItem> Items { get; set; }
    public DateTime Created { get; set; }
    public DateTime Modified { get; set; }
    public bool IsArchived { get; set; }
}