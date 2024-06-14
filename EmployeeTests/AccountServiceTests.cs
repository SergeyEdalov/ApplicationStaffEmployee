using AppStaffEmployee.Models.Dto;
using AppStaffEmployee.Services;
using AppStaffEmployee.Services.Interfaces;
using Identity.DAL.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;

namespace EmployeeTests;

[TestClass]
public class AccountServiceTests
{
    private static Mock<UserManager<User>> _mockUserManager;
    private static Mock<SignInManager<User>> _mockSignInManager;
    private static Mock<ILogger<AccountService>> _loggerMock;
    private static AccountService _accountService;
    static UserDto _userDto;

    #region Конфигурирование системы

    [ClassInitialize]
    public static void Init(TestContext context)
    {
        var userStoreMock = new Mock<IUserStore<User>>();
        _mockUserManager = new Mock<UserManager<User>>(userStoreMock.Object,
            null, null, null, null, null, null, null, null);

        var contextAccessorMock = new Mock<IHttpContextAccessor>();
        var claimsFactoryMock = new Mock<IUserClaimsPrincipalFactory<User>>();
        _mockSignInManager = new Mock<SignInManager<User>>(
            _mockUserManager.Object,
            contextAccessorMock.Object,
            claimsFactoryMock.Object,
            null, null, null, null);
        _loggerMock = new Mock<ILogger<AccountService>>();
        _accountService = new AccountService(_mockUserManager.Object, _mockSignInManager.Object, _loggerMock.Object);
        _userDto = new UserDto()
        {
            UserName = "Test",
            Password = "password",
            RememberMe = false,
        };

    }
    #endregion

    #region Тесты метода регистрации пользователя

    [TestMethod]
    public async Task Test_RegisterUser_Success()
    {
        // Arrange
        _mockUserManager.Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
               .ReturnsAsync(IdentityResult.Success);

        // Act
        var actual = await _accountService.RegisterAsync(_userDto);

        // Assert
        Assert.IsNotNull(actual);
    }
    #endregion

    #region Тесты метода логирования пользователя

    [TestMethod]
    public async Task Test_LoginUser_Success()
    {
        // Arrange
        _mockSignInManager.Setup(x => x.PasswordSignInAsync(
                   It.IsAny<string>(),
                   It.IsAny<string>(),
                   It.IsAny<bool>(),
                   It.IsAny<bool>()))
               .ReturnsAsync(SignInResult.Success);

        // Act
        var actual = await _accountService.LoginAsync(_userDto);

        // Assert
        Assert.IsNotNull(actual);
    }
    #endregion
}
