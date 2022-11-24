using System.Collections.Immutable;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        CreateUserTable(ref modelBuilder);
        CreateItemTable(ref modelBuilder);
        CreateGroceryListTable(ref modelBuilder);

        //One-To-Many Relationship
        //Many-To-Many Relationship
        
        //GroceryList-Users Relationship
        modelBuilder.Entity<UserList>()
            .HasKey(ul => new { ul.UserID, ul.GroceryListID });
        modelBuilder.Entity<UserList>()
            .HasOne(ul => ul.GroceryList)
            .WithMany(ul => ul.Users)
            .HasForeignKey(ul => ul.GroceryListID);
        modelBuilder.Entity<UserList>()
            .HasOne(ul => ul.User)
            .WithMany(ul => ul.GroceryLists)
            .HasForeignKey(ul => ul.UserID);
    }

    private void CreateGroceryListTable(ref ModelBuilder modelBuilder)
    {
        //Setting Primary Keys
        modelBuilder.Entity<GroceryList>()
            .HasKey(g => g.Id)
            .HasName("PK_GroceryList");
        
        //Auto ID generation
        modelBuilder.Entity<GroceryList>()
            .Property(l => l.Id)
            .ValueGeneratedOnAdd();
    }

    private void CreateItemTable(ref ModelBuilder modelBuilder)
    {
        //Setting Primary Keys
        modelBuilder.Entity<Item>()
            .HasKey(i => i.Id)
            .HasName("PK_Item");

        //Auto ID generation
        modelBuilder.Entity<Item>()
            .Property(i => i.Id)
            .ValueGeneratedOnAdd();
    }

    private void CreateUserTable(ref ModelBuilder modelBuilder)
    {
        //Setting Primary Keys
        modelBuilder.Entity<User>()
            .HasKey(u => u.Id)
            .HasName("PK_User");

        //Auto ID generation
        modelBuilder.Entity<User>()
            .Property(u => u.Id)
            .ValueGeneratedOnAdd();
    }
    
    public DbSet<Item> ItemTable { get; set; } = null!;
    public DbSet<User> UserTable { get; set; } = null!;
    public DbSet<GroceryList> GroceryListsTable { get; set; } = null!;
}