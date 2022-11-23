using Domain;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public class ItemRepository : IRepository<Item>
{
    private DatabaseContext _dbContext;
    private DbContextOptions<DatabaseContext> _opts;

    public ItemRepository(DatabaseContext context, DbContextOptions<DatabaseContext> opts)
    {
        _dbContext = context;
        _opts = opts;
    }

    public List<Item> All()
    {
        using (var context = new DatabaseContext(_opts))
        {
            return context.ItemTable.Include(i => i.GroceryList).ToList();
        }
    }

    public Item Create(Item t)
    {
        using (var context = new DatabaseContext(_opts))
        {
            t.GroceryList = context
                .GroceryListsTable
                .Find(t.GroceryListId) ?? throw new InvalidOperationException();
            context.ItemTable.Add(t);
            context.SaveChanges();
            return t;
        }
    }

    public Item Delete(int id)
    {
        using (var context = new DatabaseContext(_opts))
        {
            var item = context.ItemTable.Find(id);
            context.Remove(item);
            context.SaveChanges();
            return item;
        }
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