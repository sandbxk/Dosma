using System.ComponentModel.DataAnnotations;
using Application;
using Application.DTOs;
using Application.Interfaces;
using Application.Valdiators;
using Domain;
using FluentValidation;
using Infrastructure.Interfaces;
using Moq;
using ValidationException = FluentValidation.ValidationException;

namespace Tests.Services;

public class GroceryListServiceTests
{
    private IGroceryListService createGroceryListService(bool isUserAuthorized)
    {
        Mock<IRepository<GroceryList>> repository = new ();
        Mock<IUserGroceryBinding> userGroceryBinding = new ();
        
        repository.Setup(x => x.Single(It.IsAny<int>()))
            .Returns((GroceryList g) =>
            {
                g.Id = 1;
                g.Title = "";
                g.Items = new List<Item>();
                g.Users = new List<User>();
                g.SharedList = new List<UserList>();
                return g;
            });
        
        repository.Setup(x => x.Single(It.IsAny<Int32>()))
            .Returns<int>((id) => new GroceryList() { Id = 1 });
        
        repository.Setup(x => x.Delete(It.IsAny<int>())).Returns((int id) =>
        {
            return true;
        });
        
        repository.Setup(x => x.Update(It.IsAny<GroceryList>()))
            .Returns(
                (GroceryList list) =>
                {
                    return list;
                });
        
        repository.Setup(x => x.Create(It.IsAny<GroceryList>()))
            .Returns((GroceryList g) =>
            {
                g.Id = 1;
                g.Title = "Test";
                g.Items = new List<Item>();
                g.Users = new List<User>();
                g.SharedList = new List<UserList>();
                return g;
            });
        
        userGroceryBinding.Setup(x => x.IsUserInGroceryList(It.IsAny<Int32>(), It.IsAny<Int32>()))
            .Returns((int userId, int groceryListId) =>
            {
                return isUserAuthorized;
            });
        
        userGroceryBinding.Setup(x => x.AddUserToGroceryList(It.IsAny<int>(), It.IsAny<int>()))
            .Returns((int userId, int groceryListId) =>
            {
                return isUserAuthorized;
            });

        return new GroceryListService
        (
            repository.Object,
            userGroceryBinding.Object,
            new PostGroceryListValidator(),
            new PostGroceryListCreateRequestValidator(),
            new PostGroceryListUpdateRequestValidator(),
            new GroceryListValidator()
        );
    }

    [Fact]
    public void CreateList_WhenUserIsNotAuthorized_ShouldThrowException()
    {
        var listService = createGroceryListService(false);
        
        TokenUser user = new TokenUser();
        user.Id = 1;
        
        var grocerylist =  new GroceryListCreateRequest();
        grocerylist.Title = "Test";

        Assert.Throws<UnauthorizedAccessException>(() => listService.Create(grocerylist, user));
    }

    [Fact]
    public void CreateList_WithInvalidProperties_ShouldThrowValidationException()
    {
        var listService = createGroceryListService(true);
        
        TokenUser user = new TokenUser();
        user.Id = 1;
        
        Assert.Throws<ValidationException>(() => listService.Create(new GroceryListCreateRequest(), user));
    }
    
    [Fact]
    public void CreateList_WithNullObject_ShouldThrow()
    {
        var listService = createGroceryListService(true);
        
        TokenUser user = new TokenUser();
        user.Id = 1;

        Assert.Throws<ArgumentNullException>(() => listService.Create(null, user));
    }
    
    [Fact]
    public void GetListByID_WithInvalidPropegrties_ThrowValidationException()
    {
        var listService = createGroceryListService(true);
        
        Assert.Throws<ValidationException>(() => listService.GetListById(1));
        
    }
    
    
    [Fact]
    public void DeleteItem_WithAuthorizedUser_ShouldAllow()
    {
        bool isUserAuthorized = true;
        var listService = createGroceryListService(isUserAuthorized);
        
        TokenUser user = new TokenUser();
        user.Id = 1;
        
        Assert.Equal(isUserAuthorized, listService.DeleteList(1, user));
    }
    
    [Fact]
    public void DeleteItem_WithUnauthorizedUser_ShouldThrowException()
    {
        bool isUserAuthorized = false;
        var listService = createGroceryListService(isUserAuthorized);
        
        TokenUser user = new TokenUser();
        user.Id = 1;
        
        Assert.Throws<UnauthorizedAccessException>(() => (listService.DeleteList(1, user)));
    }
    
    [Fact]
    public void UpdateList_WithInvalidProperties_ThrowsValidationException()
    {
        String title = "";
        var listService = createGroceryListService(true);
        
        
        var list = new GroceryListUpdateRequest();
        list.Id = 1;
        list.Title = title;

        Assert.Throws<ValidationException>(() => listService.UpdateList(list));
    }
}