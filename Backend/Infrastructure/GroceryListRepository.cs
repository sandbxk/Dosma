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
        return _dbContext.GroceryListsTable.Include(i => i.Items).ToList();
    }

    public GroceryList Create(GroceryList t)
    {
        _dbContext.GroceryListsTable.Add(t);
        _dbContext.SaveChanges();
        return t;
    }

    public GroceryList Delete(int id)
    {
        var groceryList = _dbContext.GroceryListsTable.Find(id);
        _dbContext.GroceryListsTable.Remove(groceryList);
        _dbContext.SaveChanges();
        return groceryList;
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