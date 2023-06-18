using FluentAssertions;
using RepairShop.Data;
using RepairShop.Data.Models;
using RepairShop.Services;
using RepairShop.Services.Impl;

namespace RepairShop.Tests;

public class UserServiceTests
{
    private readonly User _standardUser = new() { Id = 1 };

    private readonly Mock<IApplicationContext> _applicationContextMock;
    private readonly IUserService _userService;


    public UserServiceTests()
    {
        _applicationContextMock = new Mock<IApplicationContext>();
        _userService = new UserService(_applicationContextMock.Object);
    }

    [Fact]
    public void GetUser_NotExistingUser_ShouldReturnNull()
    {
        var users = Array.Empty<User>();
        var mockedSet = users.GenerateDbSetMock();
        _applicationContextMock.Setup(x => x.Users).Returns(mockedSet.Object);

        var result = _userService.GetUser(It.IsAny<int>());

        result.Should().BeNull();
    }

    [Fact]
    public void GetUser_ExistingUser_ShouldReturnUser()
    {
        var users = new[] { _standardUser };
        var mockedSet = users.GenerateDbSetMock();
        _applicationContextMock.Setup(x => x.Users).Returns(mockedSet.Object);

        var result = _userService.GetUser(_standardUser.Id);

        result.Should().BeEquivalentTo(_standardUser);
    }
}