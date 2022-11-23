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
        //Setting Primary Keys
        modelBuilder.Entity<User>()
            .HasKey(u => u.Id)
            .HasName("PK_User");

        modelBuilder.Entity<GroceryList>()
            .HasKey(g => g.Id)
            .HasName("PK_GroceryList");
        
        modelBuilder.Entity<Item>()
            .HasKey(i => i.Id)
            .HasName("PK_Item");
        
        //Auto ID generation
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