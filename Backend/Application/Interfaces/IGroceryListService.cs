using Application.DTOs;
using Domain;

namespace Application.Interfaces;

public interface IGroceryListService
{
    public GroceryList Create(GroceryListDTO dto);
    public GroceryList GetListById(int id);
    public IEnumerable<GroceryList> GetListsByUser(User user);
    public IEnumerable<GroceryList> GetAllLists();

}