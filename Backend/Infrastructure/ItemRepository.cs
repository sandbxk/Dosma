using Domain;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public class ItemRepository : IRepository<Item>
{
    private DatabaseContext _dbContext;

    public ItemRepository(DatabaseContext context)
    {
        _dbContext = context;
    }

    public List<Item> All()
    {
        return _dbContext.ItemTable.Include(i => i.GroceryList).ToList();
    }

    public Item Create(Item t)
    {
        t.GroceryList = _dbContext
                .GroceryListsTable
                .Find(t.GroceryListId) ?? throw new InvalidOperationException();
        _dbContext.ItemTable.Add(t);
        _dbContext.SaveChanges();
        return t;
    }

    public Item Delete(int id)
    {
        var item = _dbContext.ItemTable.Find(id);
        _dbContext.Remove(item);
        _dbContext.SaveChanges();
        return item;
    }

    public Item Single(long id)
    {
        throw new NotImplementedException();
    }

    public Item Update(long id, Item model)
    {
        throw new NotImplementedException();
    }
}