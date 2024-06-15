using AppStaffEmployee.Controllers;
using AppStaffEmployee.Models.Dto;
using AppStaffEmployee.Services.Interfaces;
using AppStaffEmployee.ViewModels.Identity;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace EmployeeTests.ControllersTests;

[TestClass]
public class AccountControllerTests
{
    private static Mock<IMapper> _employeeMockMapper;
    private static Mock<ILogger<AccountController>> _loggerMock;
    private static Mock<IAccountService> _accountMockService;
    private AccountController _accountController;

    #region Конфигурирование системы
    [ClassInitialize]
    public static void Init(TestContext context)
    {
        _employeeMockMapper = new Mock<IMapper>();
        _loggerMock = new Mock<ILogger<AccountController>>();
        _accountMockService = new Mock<IAccountService>();

        _employeeMockMapper.Setup(x => x.Map<RegisterUserViewModel>(It.IsAny<UserDto>()))
                .Returns((UserDto src) =>
                new RegisterUserViewModel()
                {
                    UserName = src.UserName,
                    Password = src.Password,
                });
        _employeeMockMapper.Setup(x => x.Map<UserDto>(It.IsAny<RegisterUserViewModel>()))
                .Returns((RegisterUserViewModel src) =>
                new UserDto()
                {
                    UserName = src.UserName,
                    Password = src.Password,
                });
        _employeeMockMapper.Setup(x => x.Map<LoginViewModel>(It.IsAny<UserDto>()))
                .Returns((UserDto src) =>
                new LoginViewModel()
                {
                    UserName = src.UserName,
                    Password = src.Password,
                    RememberMe = src.RememberMe,
                    ReturnUrl = src.ReturnUrl,
                });
        _employeeMockMapper.Setup(x => x.Map<UserDto>(It.IsAny<LoginViewModel>()))
                .Returns((LoginViewModel src) =>
                new UserDto()
                {
                    UserName = src.UserName,
                    Password = src.Password,
                    RememberMe = src.RememberMe,
                    ReturnUrl = src.ReturnUrl,
                });
    }

    [TestInitialize]
    public void Start()
    {
        _accountController = new AccountController(_accountMockService.Object, _employeeMockMapper.Object, _loggerMock.Object);
    }
    #endregion

    #region Тесты метода Register
    [TestMethod]
    public async Task Test_RegisterPost_Success_ReturnRedirectToAction()
    {
        // Arrange
        var registerModel = new RegisterUserViewModel()
        {
            UserName = "TestUser",
            Password = "TestPassword",
            PasswordConfirmed = "TestPassword"
        };
        _accountMockService.Setup(x => x.RegisterAsync(It.IsAny<UserDto>())).ReturnsAsync(IdentityResult.Success);

        // Act
        var result = await _accountController.Register(registerModel);
        var redirectResult = result as RedirectToActionResult;

        // Assert
        Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
        Assert.IsNotNull(redirectResult);
        Assert.AreEqual("Index", redirectResult.ActionName);
        Assert.AreEqual("Home", redirectResult.ControllerName);
    }

    [TestMethod]
    public async Task Test_RegisterPost_Error_ModelIsNotValid()
    {
        // Arrange
        var registerModel = new RegisterUserViewModel();
        _accountController.ModelState.AddModelError("Name", "Required");

        // Act
        var result = await _accountController.Register(registerModel);
        var viewResult = result as ViewResult;

        // Assert
        Assert.IsInstanceOfType(result, typeof(ViewResult));
        Assert.AreEqual(viewResult?.Model, registerModel);
    }

    [TestMethod]
    public async Task Test_RegisterPost_Error_ReturnViewWithErrors()
    {
        // Arrange
        var registerModel = new RegisterUserViewModel()
        {
            UserName = "TestUser",
            Password = "TestPassword",
            PasswordConfirmed = "TestPassword"
        };
        var identityErrors = new List<IdentityError>
        {
            new IdentityError { Description = "Error 1" },
            new IdentityError { Description = "Error 2" }
        };
        var identityResult = IdentityResult.Failed(identityErrors.ToArray());
        _accountMockService.Setup(x => x.RegisterAsync(It.IsAny<UserDto>())).ReturnsAsync(identityResult);

        // Act
        var result = await _accountController.Register(registerModel);
        var viewResult = result as ViewResult;

        // Assert
        Assert.IsInstanceOfType(result, typeof(ViewResult));
        Assert.AreEqual(registerModel, viewResult.Model);
        Assert.IsTrue(viewResult.ViewData.ModelState.ContainsKey(string.Empty));
        Assert.AreEqual(2, viewResult.ViewData.ModelState[string.Empty].Errors.Count);
        Assert.AreEqual("Error 1", viewResult.ViewData.ModelState[string.Empty].Errors[0].ErrorMessage);
        Assert.AreEqual("Error 2", viewResult.ViewData.ModelState[string.Empty].Errors[1].ErrorMessage);
    }
    #endregion

    #region Тесты метода Login
    [TestMethod]
    public async Task Test_LoginPost_Success_ReturnRedirectToAction()
    {
        // Arrange
        var loginModel = new LoginViewModel()
        {
            UserName = "TestUser",
            Password = "TestPassword",
            ReturnUrl = "/Home"
        };
        _accountMockService.Setup(x => x.LoginAsync(It.IsAny<UserDto>()))
            .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);

        // Act
        var result = await _accountController.Login(loginModel);
        var redirectResult = result as LocalRedirectResult;

        // Assert
        Assert.IsInstanceOfType(result, typeof(LocalRedirectResult));
        Assert.IsNotNull(redirectResult);
        Assert.AreEqual(loginModel.ReturnUrl, redirectResult.Url);
    }

    [TestMethod]
    public async Task Test_LoginPost_Error_ModelIsNotValid()
    {
        // Arrange
        var loginModel = new LoginViewModel();
        _accountController.ModelState.AddModelError("Name", "Required");

        // Act
        var result = await _accountController.Login(loginModel);
        var viewResult = result as ViewResult;

        // Assert
        Assert.IsInstanceOfType(result, typeof(ViewResult));
        Assert.AreEqual(viewResult?.Model, loginModel);
    }

    [TestMethod]
    public async Task Test_LoginPost_Error_ReturnViewWithErrors()
    {
        // Arrange
        var loginModel = new LoginViewModel()
        {
            UserName = "TestUser",
            Password = "TestPassword",
        };
        _accountMockService.Setup(x => x.LoginAsync(It.IsAny<UserDto>()))
            .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.NotAllowed);

        // Act
        var result = await _accountController.Login(loginModel);
        var viewResult = result as ViewResult;

        // Assert
        Assert.IsInstanceOfType(result, typeof(ViewResult));
        Assert.IsNotNull(viewResult);
        Assert.IsTrue(_accountController.ModelState.ContainsKey(""));
        Assert.AreEqual("Неверное имя пользователя или пароль", _accountController.ModelState[""].Errors[0].ErrorMessage);
    }
    #endregion
}
