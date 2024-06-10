using AppStaffEmployee.Models.Dto;
using AutoMapper;
using Moq;

namespace EmployeeTests;

[TestClass]
public class EmployeeServiceTests
{
    Mock<IMapper> _employeeMockMapper;
    EmployeeServiceTests employeeService;
    EmployeeDto _employeeDto;

    [AssemblyInitialize]
    public void Init () 
    {

    }

    [TestMethod]
    public void TestMethod1()
    {
        // Arrange

        // Act

        // Assert

    }
}