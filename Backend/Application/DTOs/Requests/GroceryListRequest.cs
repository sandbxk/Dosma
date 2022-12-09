using Domain;

namespace Application.DTOs;

public class GroceryListCreateRequest
{
    public string Title { get; set; }
}

public class GroceryListUpdateRequest
{
    public int Id { get; set; }
    public string Title { get; set; }
}
