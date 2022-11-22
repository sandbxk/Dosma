namespace Domain;

public struct User
{
    public int Id { get; set; }
    public string DisplayName { get; set;}
    public string Email { get; set;}
    
    //TODO JWT
}

public struct GroceryList
{
    public int Id { get; set; }
    public string Title { get; set; }
    public IEnumerable<User> UserAccess { get; set; } // this is the list of users that have access to this list
    public IEnumerable<ListItem> Items { get; set; }
    public DateTime Created { get; set; }
    public DateTime Modified { get; set; }
    public bool IsArchived { get; set; }
}

public struct UserAccess
{
    public User User { get; set; }
    public bool CanEdit { get; set; }
    public bool CanDelete { get; set; }
}

public struct ListItem
{
    public int Id { get; set; }
    public string Title { get; set; }
    public int Quantity { get; set; }
    public ListItemStatus Status { get; set; }
    public ListItemCategory Category { get; set; }
    
    //TODO: Expected cost
}