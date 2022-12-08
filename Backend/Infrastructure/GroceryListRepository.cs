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
        var gl = _dbContext.GroceryLists.ToList();

        foreach (var groceryList in gl)
        {
            groceryList.Items = _dbContext.ItemTable.Where(i => i.GroceryListId == groceryList.Id).ToList();
            groceryList.Users = _dbContext.UserGroceryLists.Where(u => u.GroceryListID == groceryList.Id).Select(u => u.User).ToList();
        }

        return gl;
    }
    

    public GroceryList Create(GroceryList t)
    {
        if (t.Items == null)
        {
            t.Items = new List<Item>();
        }
        
        _dbContext.GroceryLists.Add(t);
        _dbContext.SaveChanges();
        return t;
    }

    public bool Delete(int id)
    {
        var groceryList = _dbContext.GroceryLists.Find(id);

        if (groceryList == null)
        {
            throw new NullReferenceException("List not found.");
        }

        _dbContext.GroceryLists.Remove(groceryList);
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
        _dbContext.GroceryLists.Update(model);
        _dbContext.SaveChanges();
        return model;
    }
}