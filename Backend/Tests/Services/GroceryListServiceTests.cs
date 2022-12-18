using Application;
using Application.DTOs;
using Application.Interfaces;
using Application.Valdiators;
using Domain;
using FluentValidation;
using Infrastructure.Interfaces;
using Moq;

namespace Tests.Services;

public class GroceryListServiceTests
{
    [Fact]
    public void GetListByID_WithInvalidPropegrties_ThrowValidationException()
    {
        String title = "";
        
        var repository = new Mock<IRepository<GroceryList>>();
        var userGroceryBinding = new Mock<IUserGroceryBinding>();
        
        var listResponseValidator = new Mock<PostGroceryListValidator>();
        var listUpdateValidator = new Mock<PostGroceryListUpdateRequestValidator>();
        var listValidator = new GroceryListValidator();
        var listCreateValidator = new Mock<IValidator<GroceryListCreateRequest>>();
        
        
        repository.Setup(x => x.Single(It.IsAny<int>()))
            .Returns((GroceryList g) =>
            {
                g.Id = 1;
                g.Title = title;
                g.Items = new List<Item>();
                g.Users = new List<User>();
                g.SharedList = new List<UserList>();
                return g;
            });

        IGroceryListService listService = new GroceryListService
        (
            repository.Object,
            userGroceryBinding.Object,
            listResponseValidator.Object,
            listCreateValidator.Object,
            listUpdateValidator.Object,
            listValidator
        );
        
        Assert.Throws<ValidationException>(() => listService.GetListById(1));
        
    }
    
    
    [Theory]
    [InlineData(true)]
    public void DeleteItem_WithAuthorizedUser_ShouldAllowOrThrowExceptionIfNot(bool isUserAuthorized)
    {
        var repository = new Mock<IRepository<GroceryList>>();
        var userGroceryBinding = new Mock<IUserGroceryBinding>();
        
        var listResponseValidator = new Mock<PostGroceryListValidator>();
        var listUpdateValidator = new Mock<PostGroceryListUpdateRequestValidator>();
        var listValidator = new Mock<GroceryListValidator>();
        
        var listCreateValidator = new Mock<IValidator<GroceryListCreateRequest>>();
        
        repository.Setup(x => x.Single(It.IsAny<Int32>()))
            .Returns<int>((id) => new GroceryList() { Id = 1 });
        
        repository.Setup(x => x.Delete(It.IsAny<int>())).Returns((int id) =>
        {
            return true;
        });
        
        userGroceryBinding.Setup(x => x.IsUserInGroceryList(It.IsAny<Int32>(), It.IsAny<Int32>()))
            .Returns((int userId, int groceryListId) =>
            {
                return isUserAuthorized;
            });

        TokenUser user = new TokenUser();
        user.Id = 1;

        IGroceryListService listService = new GroceryListService
        (
            repository.Object,
            userGroceryBinding.Object,
            listResponseValidator.Object,
            listCreateValidator.Object,
            listUpdateValidator.Object,
            listValidator.Object
        );
        
            Assert.Equal(isUserAuthorized, listService.DeleteList(1, user));
    }
    
    
    [Fact]
    public void UpdateList_WithInvalidProperties_ThrowsValidationException()
    {
        String title = "";
        
        var repository = new Mock<IRepository<GroceryList>>();
        var userGroceryBinding = new Mock<IUserGroceryBinding>();
        
        var listResponseValidator = new PostGroceryListValidator();
        var listUpdateValidator = new PostGroceryListUpdateRequestValidator();
        var listValidator = new GroceryListValidator();
        
        var listCreateValidator = new Mock<IValidator<GroceryListCreateRequest>>();

        repository.Setup(x => x.Update(It.IsAny<GroceryList>()))
            .Returns(
                (GroceryList list) =>
                {
                    return list;
                });
        

        var list = new GroceryListUpdateRequest();
        list.Id = 1;
        list.Title = title;


        IGroceryListService listService = new GroceryListService
            (
                repository.Object,
                userGroceryBinding.Object,
                listResponseValidator,
                listCreateValidator.Object,
                listUpdateValidator,
                listValidator
            );

        Assert.Throws<ValidationException>(() => listService.UpdateList(list));
    }
}