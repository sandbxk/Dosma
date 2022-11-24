using Application.DTOs;
using Domain;

namespace Application.Interfaces;

public interface IGroceryListService
{
    public GroceryList Create(GroceryListDTO dto);
    public GroceryList GetListById(int id);
    public List<GroceryList> GetListsByUser(User user);
    public List<GroceryList> GetAllLists();
    public GroceryList DeleteList(int id, GroceryList groceryList);
    public GroceryList UpdateList(int id, GroceryList groceryList);
}