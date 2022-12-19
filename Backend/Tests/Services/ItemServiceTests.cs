using Application;
using Application.DTOs;
using Application.Interfaces;
using Application.Valdiators;
using Domain;
using Infrastructure.Interfaces;
using Moq;

namespace Tests;

public class ItemServiceTests
{
    
    
    [Fact]
    public void AddItem_WithValidItem_ShouldAddItem()
    {
        var repository = new Mock<IRepository<Item>>();
        var itemValidators = new ItemValidators();
        var userGroceryBinding = new Mock<IUserGroceryBinding>();

        repository.Setup(x => x.Create(It.IsAny<Item>())).Returns((Item i) =>
        {
            return i;
        });
        
        
        var item = new Item();
        
        item.Id = 5;
        item.Title = "Test";
        item.Quantity = 1;
        item.Status = ListItemStatus.Done;
        item.Category = ListItemCategory.Butcher;
        item.GroceryListId = 5;
        
        var itemService = new ItemService(repository.Object, itemValidators, userGroceryBinding.Object);
        
        Assert.Equal(item, itemService.AddItem(item));
    }
    
    
    [Fact]
    public void CreateItem_WithNullItem_ThrowsNullException()
    {
        var repository = new Mock<IRepository<Item>>();
        var itemValidators = new Mock<ItemValidators>();
        var userGroceryBinding = new Mock<IUserGroceryBinding>();
        
        IItemService itemService = new ItemService(repository.Object, itemValidators.Object, userGroceryBinding.Object);
        
        Assert.Throws<NullReferenceException>(() => itemService.AddItem(null));
    }
    
    
    [Theory]
    [InlineData("", 1, ListItemStatus.Done, ListItemCategory.Butcher, 5)] //Empty Name
    [InlineData("Test", 0, ListItemStatus.Done, ListItemCategory.Butcher, 5)] //Invalid quantity
    [InlineData("Test", 1, ListItemStatus.Done, ListItemCategory.Butcher, 0)] //Invalid grocery list id
    public void CreateItem_WithInvalidProperties_ThrowsValidationException(String title, int quantity, ListItemStatus status, ListItemCategory category, int groceryListId)
    {
        var repository = new Mock<IRepository<Item>>();
        var itemValidators = new ItemValidators();
        var userGroceryBinding = new Mock<IUserGroceryBinding>();
        
        var item = new Item();

        item.Id = 5;
        item.Title = title;
        item.Quantity = quantity;
        item.Status = status;
        item.Category = category;
        item.GroceryListId = groceryListId;

        
        IItemService itemService = new ItemService(repository.Object, itemValidators, userGroceryBinding.Object);

        Assert.Throws<FluentValidation.ValidationException>(() => itemService.AddItem(item));
    }
    
    
    [Theory]
    [InlineData(1, true)]
    [InlineData(0, false)]
    [InlineData(-1, false)]
    public void DeleteItem_WithValidId_ShouldDeleteItem(int testValue, bool resultExpected)
    {
        var repository = new Mock<IRepository<Item>>();
        var itemValidators = new ItemValidators();
        var userGroceryBinding = new Mock<IUserGroceryBinding>();
        
        repository.Setup(x => x.Single(It.IsAny<Int32>()))
            .Returns<int>((id) => new Item() { GroceryListId = 1 });
        
        userGroceryBinding.Setup(x => x.IsUserInGroceryList(It.IsAny<Int32>(), It.IsAny<Int32>()))
            .Returns((int userId, int groceryListId) =>
            {
                return true;
            });
        
        repository.Setup(x => x.Delete(It.IsAny<int>())).Returns((int id) =>
        {
            return true;
        });
        
        TokenUser user = new TokenUser();
        user.Id = 5;
        
        var itemService = new ItemService(repository.Object, itemValidators, userGroceryBinding.Object);
        
        Assert.Equal(resultExpected, itemService.DeleteItem(testValue, user));
    }
    
    
    [Theory]
    [InlineData(true)]
    public void DeleteItem_WithAuthorizedUser_ShouldAllowOrThrowExceptionIfNot(bool isUserAuthorized)
    {
        var repository = new Mock<IRepository<Item>>();
        var itemValidators = new ItemValidators();
        var userGroceryBinding = new Mock<IUserGroceryBinding>();
        
        repository.Setup(x => x.Single(It.IsAny<Int32>()))
            .Returns<int>((id) => new Item() { GroceryListId = 1 });
        
        userGroceryBinding.Setup(x => x.IsUserInGroceryList(It.IsAny<Int32>(), It.IsAny<Int32>()))
            .Returns((int userId, int groceryListId) =>
            {
                return isUserAuthorized;
            });
        
        repository.Setup(x => x.Delete(It.IsAny<int>())).Returns((int id) =>
        {
            return true;
        });
        
        TokenUser user = new TokenUser();
        user.Id = 5;
        
        var itemService = new ItemService(repository.Object, itemValidators, userGroceryBinding.Object);
        
        Assert.Equal(isUserAuthorized, itemService.DeleteItem(1, user));
    }
    
    
    [Theory]
    [InlineData("", 1, ListItemStatus.Done, ListItemCategory.Butcher, 5)] //Empty Name
    [InlineData("Test", 0, ListItemStatus.Done, ListItemCategory.Butcher, 5)] //Invalid quantity
    [InlineData("Test", 1, ListItemStatus.Done, ListItemCategory.Butcher, 0)] //Invalid grocery list id
    public void UpdateItem_WithInvalidProperties_ThrowsValidationException(String title, int quantity, ListItemStatus status, ListItemCategory category, int groceryListId)
    {
        var repository = new Mock<IRepository<Item>>();
        var itemValidators = new ItemValidators();
        var userGroceryBinding = new Mock<IUserGroceryBinding>();
        
        var item = new Item();

        item.Id = 5;
        item.Title = title;
        item.Quantity = quantity;
        item.Status = status;
        item.Category = category;
        item.GroceryListId = groceryListId;

        
        IItemService itemService = new ItemService(repository.Object, itemValidators, userGroceryBinding.Object);

        Assert.Throws<FluentValidation.ValidationException>(() => itemService.UpdateItem(item));
    }
}