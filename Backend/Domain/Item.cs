﻿namespace Domain;

public class Item
{
    public int Id { get; set; }
    public string Title { get; set; }
    //public int Quantity { get; set; }
    //public ListItemStatus Status { get; set; }
    //public ListItemCategory Category { get; set; }
    
    public int ListId { get; set; }
    public GroceryList GroceryList { get; set; }
    
    //TODO: Expected cost
}