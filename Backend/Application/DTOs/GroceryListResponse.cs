namespace Application.DTOs;

public class GroceryListResponse
{
    public int Id {get;set;} 

    public string Title {get;set;} 

    public List<ItemResponse> Items {get;set;} 

    public List<UserResponse> Users {get;set;} 
}