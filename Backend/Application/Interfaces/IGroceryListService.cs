using Application.DTOs;
using Domain;

namespace Application.Interfaces;

public interface IGroceryListService
{
    public GroceryList Create(GroceryListDTO dto);
}