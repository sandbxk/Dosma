using Application.DTOs;
using Application.DTOs.Response;
using Domain;


namespace Application.Interfaces;

public interface IGroceryListService
{
    public GroceryListResponse Create(GroceryListCreateRequest request, TokenUser user);
    public List<GroceryListResponse> GetListsByUser(TokenUser user);

    public GroceryListResponse GetListById(int id);
    public List<GroceryListResponse> GetAllLists();
    
    public bool DeleteList(int id, TokenUser user);
    public GroceryListResponse UpdateList(GroceryListUpdateRequest groceryList);
}