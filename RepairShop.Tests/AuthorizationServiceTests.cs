using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using RepairShop.Data;
using RepairShop.Data.DTO;
using RepairShop.Data.Enums;
using RepairShop.Data.Models;
using RepairShop.Services;
using RepairShop.Services.Impl;
using RepairShop.Validation.Messages;

namespace RepairShop.Tests;

public class AuthorizationServiceTests
{
    private const string Login = "123";
    private const string Password = "123";

    private readonly Mock<IApplicationContext> _applicationContextMock;
    private readonly Mock<IServiceProvider> _serviceProviderMock;
    private readonly IAuthorizationService _authorizationService;
    private readonly User _standardUser = new() { Login = Login, Password = Password };

    public AuthorizationServiceTests()
    {
        _applicationContextMock = new Mock<IApplicationContext>();
        _serviceProviderMock = new Mock<IServiceProvider>();

        _authorizationService = new AuthorizationService(_applicationContextMock.Object, _serviceProviderMock.Object);
    }

    [Fact]
    public void AuthorizeUser_EmptySet_ShouldReturnUnsuccessfulResult()
    {
        var users = Array.Empty<User>();
        var mockedSet = users.GenerateDbSetMock();
        _applicationContextMock.Setup(x => x.Users).Returns(mockedSet.Object);

        var result = _authorizationService.AuthorizeUser(It.IsAny<string>(), It.IsAny<string>());

        result.IsFaulted.Should().Be(true);
        result.IfFail(x => x.Message.Should().Be(ValidationErrorMessages.UserDoesNotExist));
    }

    [Fact]
    public void AuthorizeUser_ExistingUser_ShouldReturnSuccessfulResult()
    {
        var users = new[] { _standardUser };
        var mockedSet = users.GenerateDbSetMock();
        _applicationContextMock.Setup(x => x.Users).Returns(mockedSet.Object);

        var result = _authorizationService.AuthorizeUser(Login, Password);

        result.IsSuccess.Should().Be(true);
        result.IfSucc(x => x.Should().BeEquivalentTo(users[0]));
    }

    [Fact]
    public void RegisterUser_FailedValidation_ShouldReturnUnsuccessfulResult()
    {
        var dto = new RegisterUserDto { Login = Login, Password = Password, RepeatPassword = Password };

        var users = Array.Empty<User>();
        var mockedSet = users.GenerateDbSetMock();
        _applicationContextMock.Setup(x => x.Users).Returns(mockedSet.Object);
        var validatorMock = new Mock<IValidator<RegisterUserDto>>();
        validatorMock.Setup(x => x.Validate(dto)).Returns(new ValidationResult(new[] { new ValidationFailure() }));
        _serviceProviderMock.Setup(x => x.GetService(typeof(IValidator<RegisterUserDto>)))
            .Returns(validatorMock.Object);

        var result = _authorizationService.RegisterClient(dto);

        result.IsFaulted.Should().Be(true);
        result.IfFail(x => x.Should().BeOfType<ValidationException>());
    }

    [Fact]
    public void RegisterUser_SuccessValidation_ShouldReturnSuccessfulResult()
    {
        var dto = new RegisterUserDto { Login = Login, Password = Password, RepeatPassword = Password };
        var expected = new User { Login = Login, Password = Password, RoleId = (int)Roles.Client };

        var users = Array.Empty<User>();
        var mockedSet = users.GenerateDbSetMock();
        _applicationContextMock.Setup(x => x.Users).Returns(mockedSet.Object);
        var validatorMock = new Mock<IValidator<RegisterUserDto>>();
        validatorMock.Setup(x => x.Validate(dto))
            .Returns(new ValidationResult());
        _serviceProviderMock.Setup(x => x.GetService(typeof(IValidator<RegisterUserDto>)))
            .Returns(validatorMock.Object);

        var result = _authorizationService.RegisterClient(dto);

        result.IsSuccess.Should().Be(true);
        result.IfSucc(x => x.Should().BeEquivalentTo(expected));
    }

    [Fact]
    public void ChangePassword_FailedValidation_ShouldReturnUnsuccessfulResult()
    {
        var dto = new ChangeUserPasswordDto() { Login = Login, NewPassword = Password, OldPassword = Password };

        var users = Array.Empty<User>();
        var mockedSet = users.GenerateDbSetMock();
        _applicationContextMock.Setup(x => x.Users).Returns(mockedSet.Object);
        var validatorMock = new Mock<IValidator<ChangeUserPasswordDto>>();
        validatorMock.Setup(x => x.Validate(dto)).Returns(new ValidationResult(new[] { new ValidationFailure() }));
        _serviceProviderMock.Setup(x => x.GetService(typeof(IValidator<ChangeUserPasswordDto>)))
            .Returns(validatorMock.Object);

        var result = _authorizationService.ChangePassword(dto);

        result.IsFaulted.Should().Be(true);
        result.IfFail(x => x.Should().BeOfType<ValidationException>());
    }

    [Fact]
    public void ChangePassword_SuccessValidation_ShouldReturnSuccessfulResult()
    {
        var dto = new ChangeUserPasswordDto() { Login = Login, NewPassword = Password, OldPassword = Password };

        var users = new[] { _standardUser };
        var mockedSet = users.GenerateDbSetMock();
        _applicationContextMock.Setup(x => x.Users).Returns(mockedSet.Object);
        var validatorMock = new Mock<IValidator<ChangeUserPasswordDto>>();
        validatorMock.Setup(x => x.Validate(dto)).Returns(new ValidationResult());
        _serviceProviderMock.Setup(x => x.GetService(typeof(IValidator<ChangeUserPasswordDto>)))
            .Returns(validatorMock.Object);

        var result = _authorizationService.ChangePassword(dto);

        result.IsSuccess.Should().Be(true);
        result.IfSucc(x => x.Should().BeTrue());
    }
}