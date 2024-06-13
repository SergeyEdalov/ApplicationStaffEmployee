using AppStaffEmployee.Models;
using AppStaffEmployee.Models.Database;
using AppStaffEmployee.Models.Dto;
using AppStaffEmployee.Services;
using AutoMapper;
using EmployeeTests.TestDbContext;
using Microsoft.EntityFrameworkCore;
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

    #region Конфигурирование системы
    [ClassInitialize]
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
        _employeeMockMapper.Setup(x => x.Map(It.IsAny<EmployeeDto>(), It.IsAny<EmployeeModel>()))
                .Returns((EmployeeDto src, EmployeeModel dest) =>
                {
                    dest.Id = src.Id ?? dest.Id;
                    dest.FullName = src.FullName;
                    dest.Birthday = src.Birthday;
                    dest.Department = src.Department;
                    dest.JobTitle = src.JobTitle;
                    dest.WorkStart = src.WorkStart;
                    dest.Salary = src.Salary;
                    return dest;
                });

        _employeeService = new EmployeeService(_employeeContext, _employeeMockMapper.Object, _loggerMock.Object);
        _employeeDto = new EmployeeDto();
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
    #endregion

    #region Тесты метода получения идентификатора сотрудника
    [TestMethod]
    public async Task Test_GetEmpoloyeeByID_Success()
    {
        // Arrange
        var employeeId = new Guid("67bbefef-a586-4416-b5f5-8d70f3b51d44");

        // Act
        var actual = await _employeeService.GetEmpoloyeeByIDAsync(employeeId);

        // Assert
        Assert.AreEqual(employeeId, actual.Id);
    }

    [TestMethod]
    public async Task Test_GetEmpoloyeeByID_NullId()
    {
        // Arrange
        var employeeId1 = new Guid("67bbefef-a586-4416-b5f5-8d70f3b51d56");
        var employeeId2 = new Guid();
        // Act
        var actual1 = await _employeeService.GetEmpoloyeeByIDAsync(employeeId1);
        var actual2 = await _employeeService.GetEmpoloyeeByIDAsync(employeeId2);

        // Assert
        Assert.IsNull(actual1);
        Assert.IsNull(actual2);
    }
    #endregion

    #region Тесты метода добавления сотрудника
    [TestMethod]
    public async Task Test_AddEmpoloyee_Success()
    {
        // Arrange
        _employeeDto = new EmployeeDto
        {
            FullName = "Чертополохова Анастасия Николаевна",
            Birthday = new DateTime(1965, 3, 20),
            Department = "Сметный",
            JobTitle = "Сметчик",
            WorkStart = new DateTime(2006, 5, 18),
            Salary = 82000.0M
        };
        var expectedCountEmployees = _employeeContext.Employees.Count();

        // Act
        var actualEmployeeId = await _employeeService.AddEmployeeAsync(_employeeDto);
        var expectedEmployee = await _employeeContext.Employees
            .FirstOrDefaultAsync(x => x.FullName.Equals("Чертополохова Анастасия Николаевна"));
        var actualCountEmployees = _employeeContext.Employees.Count();

        // Assert
        Assert.AreEqual(expectedEmployee.FullName, _employeeDto.FullName);
        Assert.AreEqual(expectedCountEmployees + 1, actualCountEmployees);
        Assert.IsNotNull(actualEmployeeId);
    }

    [TestMethod]
    //[ExpectedException(typeof(Exception))]
    public async Task Test_AddEmpoloyee_Exception()
    {
        // Arrange
        _employeeDto = new EmployeeDto();
        _employeeDto.FullName = null;
        _employeeDto.Birthday = DateTime.Parse("1965, 3, 20");
        _employeeDto.Department = "Сметный";
        _employeeDto.JobTitle = "Сметчик";
        _employeeDto.WorkStart = new DateTime(2006, 5, 18);
        _employeeDto.Salary = 82000.0M;
        var expectedCountEmployees = _employeeContext.Employees.Count();

        Exception ex = new Exception();
        // Act
        //Assert.ThrowsExceptionAsync<Exception>(async () => await _employeeService.AddEmployeeAsync(employeeDto));
        //CleanUp();
        var exception = Assert.ThrowsExceptionAsync<Exception>(async () => await _employeeService.AddEmployeeAsync(_employeeDto)).Result;
        //Init();
        var employeeModel = _employeeMockMapper.Object.Map<EmployeeModel>(_employeeDto);
        //_employeeContext.Entry(employeeModel).State = EntityState.Detached;
        _employeeContext.Employees.Remove(employeeModel);
        _employeeContext.SaveChanges();
        // Assert
        Assert.IsNotNull(exception);
        Assert.AreEqual("Ошибка добавления сотрудника", exception.Message);
    }
    #endregion

    #region Тесты метода редактирования сотрудника
    [TestMethod]
    public async Task Test_EditEmpoloyee_Success()
    {
        // Arrange
        _employeeDto.Id = Guid.Parse("bc3d5e78-fe17-4e95-b08e-a56d58384325"); //id Popova
        _employeeDto.FullName = "Мохова Ирина Владимировна";
        _employeeDto.Birthday = DateTime.Parse("1965, 3, 20");
        _employeeDto.Department = "Сметный";
        _employeeDto.JobTitle = "Сметчик";
        _employeeDto.WorkStart = new DateTime(2006, 5, 18);
        _employeeDto.Salary = 51000.0M;
        var expectedEmployee = _employeeContext.Employees.FirstOrDefault(x => x.Id == _employeeDto.Id);

        // Act
        var actual = await _employeeService.EditEmployeeAsync(_employeeDto);

        // Assert
        Assert.IsTrue(actual);
        Assert.AreEqual(expectedEmployee.FullName, _employeeDto.FullName);
    }
    #endregion

    #region Тесты метода удаления сотрудника

    [TestMethod]
    public async Task Test_RemoveEmpoloyee_Success()
    {
        // Arrange
        var employeeId = new Guid("db51e04d-e114-4e7f-bfe2-87ab10a48bbf");
        var expectedCountEmployees = _employeeContext.Employees.Count();

        // Act
        var actualResult = await _employeeService.RemoveEmployeeAsync(employeeId);
        var actualCountEmployees = _employeeContext.Employees.Count();

        // Assert
        Assert.IsTrue(actualResult);
        Assert.AreEqual(expectedCountEmployees - 1, actualCountEmployees);
    }

    [TestMethod]
    public async Task Test_RemoveEmpoloyee_EmployeeNotFound()
    {
        // Arrange
        var employeeId1 = new Guid("db51e04d-e114-4e7f-bfe2-87ab10a48851");
        var employeeId2 = new Guid();

        // Act
        var actualResult1 = await _employeeService.RemoveEmployeeAsync(employeeId1);
        var actualResult2 = await _employeeService.RemoveEmployeeAsync(employeeId2);

        // Assert
        Assert.IsFalse(actualResult1);
        Assert.IsFalse(actualResult2);
    }

    #endregion

    #region Тесты метода получения списка сотрудников

    [TestMethod]
    public async Task Test_GetListEmpoloyees_Success()
    {
        // Arrange
        string expectedSortOrder = null;
        string expectedSortField = null;
        string expectedSearchString = null;
        var expectedCountEmployees = _employeeContext.Employees.Count();

        // Act
        var actualResult = await _employeeService.GetSortedFilteredEmployeesAsync(expectedSortOrder, expectedSortField, expectedSearchString);

        // Assert
        Assert.AreEqual(expectedCountEmployees, actualResult.Count());
    }

    [TestMethod]
    public async Task Test_GetListEmpoloyeesWithSortAndFilter_Success()
    {
        // Arrange
        string expectedSortOrder = "asc";
        string expectedSortField = "Salary";
        string expectedSearchString = "chief";

        // Act
        var actualResult = await _employeeService.GetSortedFilteredEmployeesAsync(expectedSortOrder, expectedSortField, expectedSearchString);

        // Assert
        Assert.IsTrue(actualResult.Count() == 2);
        Assert.IsTrue(actualResult.First().Salary.CompareTo(actualResult.Last().Salary) < 0);
    }
    #endregion
}