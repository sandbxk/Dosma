using Domain;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .Property(u => u.Id)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<GroceryList>()
            .Property(l => l.Id)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<Item>()
            .Property(i => i.Id)
            .ValueGeneratedOnAdd();
    }
    public DbSet<Item> ListItemTable { get; set; }

    public DbSet<User> UserTable { get; set; }
    public DbSet<GroceryList> GroceryListsTable { get; set; }
}