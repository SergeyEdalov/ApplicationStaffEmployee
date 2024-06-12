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
    public async Task Test_GetEmpoloyeeByID_Succsess()
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
    public async Task Test_AddEmpoloyee_Succsess()
    {
        // Arrange
        _employeeDto = new EmployeeDto();
        _employeeDto.FullName = "Чертополохова Анастасия Николаевна";
        _employeeDto.Birthday = new DateTime(1965, 3, 20);
        _employeeDto.Department = "Сметный";
        _employeeDto.JobTitle = "Сметчик";
        _employeeDto.WorkStart = new DateTime(2006, 5, 18);
        _employeeDto.Salary = 82000.0M;
        

        // Act
        var employeeIdExpected= await _employeeService.AddEmployeeAsync(_employeeDto);
        var expected = await _employeeContext.Employees
            .FirstOrDefaultAsync(x => x.FullName.Equals("Чертополохова Анастасия Николаевна"));

        // Assert
        Assert.AreEqual(expected.FullName, _employeeDto.FullName);
        Assert.IsNotNull(employeeIdExpected);
    }

    //[TestMethod]
    ////[ExpectedException(typeof(Exception))]
    //public async Task Test_AddEmpoloyee_Exception()
    //{
    //    // Arrange
    //    _employeeDto.FullName = null;
    //    _employeeDto.Birthday = DateTime.Parse("1965, 3, 20");
    //    _employeeDto.Department = "Сметный";
    //    _employeeDto.JobTitle = "Сметчик";
    //    _employeeDto.WorkStart = new DateTime(2006, 5, 18);
    //    _employeeDto.Salary = 82000.0M;

    //    Exception ex = new Exception();
    //    // Act
    //    //Assert.ThrowsExceptionAsync<Exception>(async () => await _employeeService.AddEmployeeAsync(employeeDto));
    //    //CleanUp();
    //    var exception = Assert.ThrowsExceptionAsync<Exception>(async () => await _employeeService.AddEmployeeAsync(_employeeDto)).Result;
    //    //Init();
    //    var employeeModel = _employeeMockMapper.Object.Map<EmployeeModel>(_employeeDto);
    //    //_employeeContext.Entry(employeeModel).State = EntityState.Detached;
    //    _employeeContext.Employees.Remove(employeeModel);
    //    _employeeContext.SaveChanges();
    //    // Assert
    //    Assert.IsNotNull(exception);
    //    Assert.AreEqual("Ошибка добавления сотрудника", exception.Message);
    //}
    #endregion


    [TestMethod]
    public async Task Test_EditEmpoloyee_Succsess()
    {
        // Arrange
        _employeeDto.Id = Guid.Parse("6afd4cd6-0fe0-4d0b-aa72-bd4bf97a4860"); //id Коноваловой
        _employeeDto.FullName = "Мохова Ирина Владимировна";
        _employeeDto.Birthday = DateTime.Parse("1965, 3, 20");
        _employeeDto.Department = "Сметный";
        _employeeDto.JobTitle = "Сметчик";
        _employeeDto.WorkStart = new DateTime(2006, 5, 18);
        _employeeDto.Salary = 51000.0M;

        // Act
        var actual = await _employeeService.EditEmployeeAsync(_employeeDto);
        var expectedEmployee = _employeeContext.Employees.FirstOrDefault(x => x.Id == Guid.Parse("6afd4cd6-0fe0-4d0b-aa72-bd4bf97a4860"));

        // Assert
        Assert.IsTrue(actual);
        Assert.AreEqual(expectedEmployee.FullName, _employeeDto.FullName);
    }
}