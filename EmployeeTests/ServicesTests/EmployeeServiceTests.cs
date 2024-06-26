using AppStaffEmployee.Models;
using AppStaffEmployee.Models.Dto;
using AppStaffEmployee.Services;
using AutoMapper;
using EmployeeTests.TestDbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace EmployeeTests.ServicesTests;

[TestClass]
public class EmployeeServiceTests : TestCommandBase
{
    private static Mock<IMapper> _employeeMockMapper;
    private static Mock<ILogger<EmployeeService>> _loggerMock;
    private EmployeeService _employeeService;
    private EmployeeDto _employeeDto;

    #region ���������������� �������
    [ClassInitialize]
    public static void Init(TestContext context)
    {
        _employeeMockMapper = new Mock<IMapper>();
        _loggerMock = new Mock<ILogger<EmployeeService>>();
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
    }

    [TestInitialize]
    public void Start()
    {
        _employeeContext = getEContext();
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

    #region ����� ������ ��������� �������������� ����������
    [TestMethod]
    public async Task Test_GetEmpoloyeeByID_Success_ReturnEmployeeId()
    {
        // Arrange
        var employeeId = new Guid("67bbefef-a586-4416-b5f5-8d70f3b51d44");

        // Act
        var result = await _employeeService.GetEmpoloyeeByIDAsync(employeeId);

        // Assert
        Assert.AreEqual(employeeId, result.Id);
    }

    [TestMethod]
    public async Task Test_GetEmpoloyeeByID_Error_ReturnNullId()
    {
        // Arrange
        var employeeId1 = new Guid("67bbefef-a586-4416-b5f5-8d70f3b51d56");
        var employeeId2 = new Guid();
        // Act
        var result1 = await _employeeService.GetEmpoloyeeByIDAsync(employeeId1);
        var result2 = await _employeeService.GetEmpoloyeeByIDAsync(employeeId2);

        // Assert
        Assert.IsNull(result1);
        Assert.IsNull(result2);
    }
    #endregion

    #region ����� ������ ���������� ����������
    [TestMethod]
    public async Task Test_AddEmpoloyee_Success_ReturnEmployeeId()
    {
        // Arrange
        _employeeDto.FullName = "������������� ��������� ����������";
        _employeeDto.Birthday = new DateTime(1965, 3, 20);
        _employeeDto.Department = "�������";
        _employeeDto.JobTitle = "�������";
        _employeeDto.WorkStart = new DateTime(2006, 5, 18);
        _employeeDto.Salary = 82000.0M;
        var expectedCountEmployees = _employeeContext.Employees.Count();

        // Act
        var actualEmployeeId = await _employeeService.AddEmployeeAsync(_employeeDto);
        var expectedEmployee = await _employeeContext.Employees
            .FirstOrDefaultAsync(x => x.FullName.Equals("������������� ��������� ����������"));
        var actualCountEmployees = _employeeContext.Employees.Count();

        // Assert
        Assert.AreEqual(expectedEmployee.FullName, _employeeDto.FullName);
        Assert.AreEqual(expectedCountEmployees + 1, actualCountEmployees);
        Assert.IsNotNull(actualEmployeeId);
    }

    [TestMethod]
    public async Task Test_AddEmpoloyee_Error_ReturnException()
    {
        // Arrange
        // Act
        var exception = await Task.Run(() => Assert.ThrowsExceptionAsync<Exception>(async () =>
            await _employeeService.AddEmployeeAsync(_employeeDto)).Result);

        // Assert
        Assert.IsNotNull(exception);
        Assert.AreEqual("������ ���������� ����������", exception.Message);
    }
    #endregion

    #region ����� ������ �������������� ����������
    [TestMethod]
    public async Task Test_EditEmpoloyee_Success_ReturnTrue()
    {
        // Arrange
        _employeeDto.Id = Guid.Parse("bc3d5e78-fe17-4e95-b08e-a56d58384325"); //id Popova
        _employeeDto.FullName = "������ ����� ������������";
        _employeeDto.Birthday = DateTime.Parse("1965, 3, 20");
        _employeeDto.Department = "�������";
        _employeeDto.JobTitle = "�������";
        _employeeDto.WorkStart = new DateTime(2006, 5, 18);
        _employeeDto.Salary = 51000.0M;
        var expectedEmployee = _employeeContext.Employees.FirstOrDefault(x => x.Id == _employeeDto.Id);

        // Act
        var result = await _employeeService.EditEmployeeAsync(_employeeDto);

        // Assert
        Assert.IsTrue(result);
        Assert.AreEqual(expectedEmployee.FullName, _employeeDto.FullName);
    }

    [TestMethod]
    public async Task Test_EditEmpoloyee_Error_ReturnFalse_NullEmployee()
    {
        // Arrange
        // Act
        var result = await _employeeService.EditEmployeeAsync(_employeeDto);

        // Assert
        Assert.IsFalse(result);
    }
    #endregion

    #region ����� ������ �������� ����������
    [TestMethod]
    public async Task Test_RemoveEmpoloyee_Success_ReturnTrue()
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
    public async Task Test_RemoveEmpoloyee_Error_EmployeeNotFound_ReturnFalse()
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

    #region ����� ������ ��������� ������ �����������
    [TestMethod]
    public async Task Test_GetListEmpoloyees_Success_ReturnListEmployees()
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
    public async Task Test_GetListEmpoloyeesWithSortAndFilter_Success_ReturnListEmployees()
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