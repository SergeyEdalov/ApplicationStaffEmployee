using AppStaffEmployee.Controllers;
using AppStaffEmployee.Models.Dto;
using AppStaffEmployee.Services.Interfaces;
using AppStaffEmployee.ViewModels.Identity;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;

namespace EmployeeTests;

[TestClass]
public class AccountControllerTests
{
    private Mock<IMapper> _employeeMockMapper;
    private Mock<ILogger<AccountController>> _loggerMock;
    private Mock<IAccountService> _accountMockService;
    private AccountController _accountController;

    #region Конфигурирование системы
    [TestInitialize]
    public void Init()
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

        _accountController = new AccountController(_accountMockService.Object, _employeeMockMapper.Object, _loggerMock.Object);
    }
    #endregion

    #region Тесты метода Register
    [TestMethod]
    public async Task Test_Index_Success_ReturnEmployeeViewModel()
    {
        // Arrange

        // Act


        // Assert

    }


    #endregion

}
