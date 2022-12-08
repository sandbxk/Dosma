
namespace Application.DTOs;


public class GroceryListUpdateRequest
{
    public int Id {get;set;}
    public string Title {get;set;}
}

public class GroceryListCreateRequest 
{
    public string Title {get;set;} = "";

    public int UserId {get;set;} 
}

public class GroceryListAddUserRequest 
{
    public int ListId {get;set;} 

    public int UserId {get;set;} 
}
