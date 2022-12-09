using Application.DTOs;
using Domain;

namespace Application.Interfaces;

public interface IGroceryListService
{
    public GroceryListResponse Create(GroceryListCreateRequest dto);
    public List<GroceryListResponse> GetListsByUser(User user);
    public List<GroceryListResponse> GetAllLists();
    public bool DeleteList(int id);
    public GroceryListResponse UpdateList(int id, GroceryList groceryList);

    public GroceryListResponse GetListById(int id);
}