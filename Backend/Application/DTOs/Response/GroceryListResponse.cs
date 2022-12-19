using Domain;

namespace Application.DTOs.Response;

public class GroceryListResponse
{
    public int Id { get; set; }
    public String Title { get; set; }
    public List<UserResponse> Users { get; set; }
    public List<ItemResponse> Items { get; set; }
}