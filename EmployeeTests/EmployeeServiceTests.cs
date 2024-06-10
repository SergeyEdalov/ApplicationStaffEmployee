using AppStaffEmployee.Models;
using AppStaffEmployee.Models.Dto;
using AppStaffEmployee.Services;
using AutoMapper;
using EmployeeTests.TestDbContext;
using Microsoft.Extensions.Logging;
using Moq;

namespace EmployeeTests;

[TestClass]
public class EmployeeServiceTests : TestCommandBase
{
    private static Mock<IMapper> _employeeMockMapper;
    private static Mock<ILogger<EmployeeService>> _loggerMock;
    private static EmployeeService _employeeService;
    private static EmployeeDto _employeeDto;

    [AssemblyInitialize]
    public static void Init(TestContext context)
    {
        _employeeMockMapper = new Mock<IMapper>();
        _loggerMock = new Mock<ILogger<EmployeeService>>();
        _employeeContext = getEContext();
        _employeeMockMapper.Setup(x => x.Map<EmployeeModel>(It.IsAny<EmployeeDto>()))
                .Returns((EmployeeDto src) =>
                new EmployeeModel()
                {
                    Id = (Guid)src.Id,
                    FullName = src.FullName,
                    Birthday = src.Birthday,
                    Department = src.Department,
                    JobTitle = src.JobTitle,
                    WorkStart = src.WorkStart,
                    Salary = src.Salary,
                });
        _employeeMockMapper.Setup(x => x.Map<EmployeeDto>(It.IsAny<EmployeeModel>()))
                .Returns((EmployeeModel src) =>
                new EmployeeDto()
                {
                    Id = src.Id,
                    FullName = src.FullName,
                    Birthday = src.Birthday,
                    Department = src.Department,
                    JobTitle = src.JobTitle,
                    WorkStart = src.WorkStart,
                    Salary = src.Salary,
                });

        _employeeService = new EmployeeService(_employeeContext, _employeeMockMapper.Object, _loggerMock.Object);
    }
    [AssemblyCleanup]
    public static void CleanUp()
    {
        if (_employeeContext != null)
        {
            Destroy(_employeeContext);
            _employeeContext = null;
        }
    }

    [TestMethod]
    public async Task Test_Succsess_GetEmpoloyeeByID()
    {
        // Arrange
        var employeeId = new Guid("67bbefef-a586-4416-b5f5-8d70f3b51d44");
        // Act
        var actual = await _employeeService.GetEmpoloyeeByIDAsync(employeeId);

        // Assert
        Assert.AreEqual(employeeId, actual.Id);
    }
}