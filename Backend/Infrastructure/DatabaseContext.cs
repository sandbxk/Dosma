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
        modelBuilder.Entity<GroceryList>()
            .Property(l => l.Id)
            .ValueGeneratedOnAdd();
    }
    
    public DbSet<GroceryList> GroceryLists { get; set; }
}