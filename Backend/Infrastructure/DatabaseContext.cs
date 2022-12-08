using System.Collections.Immutable;
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Setting Primary Keys
        modelBuilder.Entity<Item>()
            .HasKey(i => i.Id)
            .HasName("PK_Item");

        //Auto ID generation
        modelBuilder.Entity<Item>()
            .Property(i => i.Id)
            .ValueGeneratedOnAdd();

        //Required Properties
        modelBuilder.Entity<User>()
            .Property(u => u.DisplayName)
            .HasColumnType("TEXT")
            .IsUnicode(true)
            .IsRequired(true)
            .HasMaxLength(50);

        modelBuilder.Entity<GroceryList>()
            .Property(g => g.Title)
            .HasColumnType("TEXT")
            .IsUnicode(true)
            .IsRequired()
            .HasMaxLength(100);

        modelBuilder.Entity<Item>()
            .Property(i => i.Title)
            .HasColumnType("TEXT")
            .IsUnicode(true)
            .IsRequired(true)
            .HasMaxLength(50);

        modelBuilder.Entity<Item>()
            .Property(i => i.Status)
            .HasConversion(c => c.ToString(), c => (ListItemStatus)Enum.Parse(typeof(ListItemStatus), c));
        
        modelBuilder.Entity<Item>()
            .Property(i => i.Category)
            .HasConversion(c => c.ToString(), c => (ListItemCategory)Enum.Parse(typeof(ListItemCategory), c));
    

        /**
         * One-To-Many Relationship
         */
        //Item to GroceryList
        modelBuilder.Entity<GroceryList>()
            .HasMany(i => i.Items);


        /**
         * Many-To-Many Relationship
         */
        //GroceryList-User
        modelBuilder.Entity<UserGroceryList>()
            .HasKey(ul => new { ul.UserID, ul.GroceryListID });


        // Joined to User
        modelBuilder.Entity<UserGroceryList>()
            .HasOne(ul => ul.User);

        // Joined to GroceryList
        modelBuilder.Entity<UserGroceryList>()
            .HasOne(ul => ul.GroceryList);

        // User to Joined
        modelBuilder.Entity<User>()
            .HasMany(u => u.GroceryLists);

        // GroceryList to Joined
        modelBuilder.Entity<GroceryList>()
            .HasMany(g => g.Users);
    }
    
    public DbSet<Item> ItemTable { get; set; } = null!;
    

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<GroceryList> GroceryLists { get; set; } = null!;
    public DbSet<UserGroceryList> UserGroceryLists { get; set; } = null!;
}