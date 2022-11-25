using Domain;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

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
        throw new NotImplementedException();
    }

    public Item Create(Item t)
    {
        GroceryList groceryList = _dbContext.GroceryListsTable.Include(l => l.Items).ToList().Find(l => l.Id == t.GroceryListId) ?? throw new InvalidOperationException();
        Item item = new Item();
        item.Title = t.Title;
        item.GroceryListId = t.GroceryListId;
        
        groceryList.Items.Add(item);
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