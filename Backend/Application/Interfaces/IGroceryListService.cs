using Application.DTOs;
using Domain;

namespace Application.Interfaces;

public interface IGroceryListService
{
    public GroceryList Create(GroceryListDTO dto);
    public List<GroceryList> GetListsByUser(User user);
    public List<GroceryList> GetAllLists();
    public bool DeleteList(GroceryList groceryList);
    public GroceryList UpdateList(int id, GroceryList groceryList);
}