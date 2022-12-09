using Application.DTOs;
using Application.DTOs.Response;
using Domain;


namespace Application.Interfaces;

public interface IGroceryListService
{
    public GroceryList Create(GroceryListResponse response);
    public List<GroceryList> GetListsByUser(User user);
    
    public GroceryListResponse GetListById(int id);
    public List<GroceryList> GetAllLists();
    public bool DeleteList(GroceryList groceryList);
    public GroceryList UpdateList(int id, GroceryList groceryList);
}