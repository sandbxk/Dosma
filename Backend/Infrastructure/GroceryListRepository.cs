﻿using System.ComponentModel.DataAnnotations;
using Domain;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class GroceryListRepository : IRepository<GroceryList>
{
    private DatabaseContext _dbContext;
    
    public GroceryListRepository(DatabaseContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public List<GroceryList> All()
    {
        return _dbContext.GroceryListsTable
            .Include(l => l.Items)
            .Include(l => l.Users)
            .ToList();
    }

    public GroceryList Create(GroceryList t)
    {
        if (t.Items == null)
        {
            t.Items = new List<Item>();
        }
        
        _dbContext.GroceryListsTable.Add(t);
        _dbContext.SaveChanges();
        return t;
    }

    public bool Delete(int id)
    {
        var groceryList = _dbContext.GroceryListsTable.Find(id);

        if (groceryList == null)
        {
            throw new NullReferenceException("List not found.");
        }

        _dbContext.GroceryListsTable.Remove(groceryList);
        int change = _dbContext.SaveChanges();
        
        if (change == 0)
        {
            throw new NullReferenceException("List not found.");
        }
        return true;
    }

    public GroceryList Single(long id)
    {
        throw new NotImplementedException();
    }

    public GroceryList Update(GroceryList model)
    {
        _dbContext.GroceryListsTable.Update(model);
        _dbContext.SaveChanges();
        return model;
    }
}