using System.Collections.Immutable;
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

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

        /**
         * One-To-Many Relationship
         */
        //Item to GroceryList
        modelBuilder.Entity<Item>()
            .HasOne(i => i.GroceryList)
            .WithMany(i => i.Items)
            .HasForeignKey(i => i.GroceryListId)
            .OnDelete(DeleteBehavior.Cascade);


        /**
         * Many-To-Many Relationship
         */
        //GroceryList-Userr

    }
    
    public DbSet<Item> ItemTable { get; set; }
    public DbSet<User> UserTable { get; set; }
    public DbSet<GroceryList> GroceryListsTable { get; set; }
}