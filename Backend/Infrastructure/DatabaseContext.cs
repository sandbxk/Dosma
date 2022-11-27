﻿using System.Collections.Immutable;
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
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
            .WithMany(ul => ul.GroceryLists);

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

    }
    
    public DbSet<Item> ItemTable { get; set; } = null!;
    public DbSet<User> UserTable { get; set; } = null!;
    public DbSet<GroceryList> GroceryListsTable { get; set; } = null!;
}