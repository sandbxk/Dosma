using System.Collections.Immutable;
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) {}
    
    override protected void OnModelCreating(ModelBuilder modelBuilder)
    {
        SetupItemData(ref modelBuilder);
        SetupListData(ref modelBuilder);
        SetupUserData(ref modelBuilder);
        
        BindItemToGroceryList(ref modelBuilder);
        BindUserToGroceryList(ref modelBuilder);
    }

    private void BindUserToGroceryList(ref ModelBuilder modelBuilder)
    {
        // shared primary key
        modelBuilder.Entity<UserGroceryList>()
            .HasKey(ul => new { ul.UserID, ul.GroceryListID });

        modelBuilder.Entity<User>()
            .HasMany( u => u.SharedList)
            .WithOne(u => u.User);

        modelBuilder.Entity<GroceryList>()
            .HasMany(u => u.SharedList)
            .WithOne(l => l.GroceryList);

        modelBuilder.Entity<UserGroceryList>()
            .HasOne(u => u.User)
            .WithMany(ul => ul.SharedList);

        modelBuilder.Entity<UserGroceryList>()
            .HasOne(gl => gl.GroceryList)
            .WithMany(ul => ul.SharedList);
    }

    private void BindItemToGroceryList(ref ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<GroceryList>()
            .HasMany(l => l.Items);
    }

    private void SetupItemData(ref ModelBuilder modelBuilder)
    {
          //Setting Primary Keys
        modelBuilder.Entity<Item>()
            .HasKey(i => i.Id)
            .HasName("PK_Item");

        //Auto ID generation
        modelBuilder.Entity<Item>()
            .Property(i => i.Id)
            .ValueGeneratedOnAdd();

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
    }

    private void SetupListData(ref ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<GroceryList>()
            .HasKey(g => g.Id)
            .HasName("PK_GroceryList");

        modelBuilder.Entity<GroceryList>()
            .Property(g => g.Id)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<GroceryList>()
            .Property(g => g.Title)
            .HasColumnType("TEXT")
            .IsUnicode(true)
            .IsRequired()
            .HasMaxLength(100);
    }

    private void SetupUserData(ref ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasKey(u => u.Id)
            .HasName("PK_User");

        modelBuilder.Entity<User>()
            .Property(u => u.Id)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<User>()
            .Property(u => u.Username)
            .HasColumnType("TEXT")
            .IsUnicode(true)
            .IsRequired(true)
            .HasMaxLength(50);

        modelBuilder.Entity<User>()
            .Property(u => u.DisplayName)
            .HasColumnType("TEXT")
            .IsUnicode(true)
            .IsRequired(true)
            .HasMaxLength(50);
        
        modelBuilder.Entity<User>()
            .Property(u => u.HashedPassword)
            .HasColumnType("TEXT")
            .IsUnicode(true)
            .IsRequired(true)
            .HasMaxLength(128);
        
        modelBuilder.Entity<User>()
            .Property(u => u.Salt)
            .HasColumnType("TEXT")
            .IsUnicode(true)
            .IsRequired(true)
            .HasMaxLength(128);
    }

    public DbSet<Item> ItemTable { get; set; } = null!;
    public DbSet<User> UserTable { get; set; } = null!;
    public DbSet<GroceryList> GroceryListsTable { get; set; } = null!;
    public DbSet<UserGroceryList> UserGroceryListsTable { get; set; } = null!;
}