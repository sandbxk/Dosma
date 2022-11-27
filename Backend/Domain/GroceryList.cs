﻿namespace Domain;

public class GroceryList
{
    public int Id { get; set; }
    public string Title { get; set; }
    
    public List<Item> Items { get; set; }
    
    public List<UserList> Users { get; set; }
}
