using ApplicationStaffEmployee.Controllers;
using AppStaffEmployee.Models;
using AppStaffEmployee.Models.Dto;
using AppStaffEmployee.Services;
using AppStaffEmployee.Services.Interfaces;
using AppStaffEmployee.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;

namespace EmployeeTests;

[TestClass]
public class EmployeeControllerTests
{
    private static Mock<IMapper> _employeeMockMapper;
    private static Mock<ILogger<EmployeeController>> _loggerMock;
    private static Mock<IEmployeeService<EmployeeDto, Guid>> _employeeMockService;
    private static EmployeeController _employeeController;

    #region Конфигурирование системы
    [ClassInitialize]
    public static void Init(TestContext context)
    {
        _employeeMockMapper = new Mock<IMapper>();
        _loggerMock = new Mock<ILogger<EmployeeController>>();
        _employeeMockService = new Mock<IEmployeeService<EmployeeDto, Guid>>();

        _employeeMockMapper.Setup(x => x.Map<EmployeeViewModel>(It.IsAny<EmployeeDto>()))
                .Returns((EmployeeDto src) =>
                new EmployeeViewModel()
                {
                    Id = (Guid)src.Id,
                    FullName = src.FullName,
                    Birthday = src.Birthday,
                    Department = src.Department,
                    JobTitle = src.JobTitle,
                    WorkStart = src.WorkStart,
                    Salary = src.Salary,
                });
        _employeeMockMapper.Setup(x => x.Map<EmployeeDto>(It.IsAny<EmployeeViewModel>()))
                .Returns((EmployeeViewModel src) =>
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

        _employeeController = new EmployeeController(_employeeMockService.Object, _loggerMock.Object, _employeeMockMapper.Object);

    }
    #endregion

    #region Тесты метода Index 
    [TestMethod]
    public async Task Test_Index_Success_ReturnViewModel()
    {
        // Arrange
        string sortOrder = null;
        string sortField = null;
        string searchString = null;
        var expectedEmployees = new List<EmployeeDto>
    {
        new EmployeeDto
        {
            Id = Guid.Parse("6afd4cd6-0fe0-4d0b-aa72-bd4bf97a4860"),
            FullName = "Konovalova Irina Alekseevna",
            Birthday = new DateTime(1978, 3, 30),
            Department = "bookkeeping",
            JobTitle = "chief accountant",
            WorkStart = new DateTime(2014, 6, 6),
            Salary = 90000.0M
        },
        new EmployeeDto
        {
            Id = Guid.Parse("a0cf8d7c-f8a1-460a-9b97-14a58fae574f"),
            FullName = "Ermakov Sergey Vasilevich",
            Birthday = new DateTime(1990, 5, 28),
            Department = "financial",
            JobTitle = "financial director",
            WorkStart = new DateTime(2022, 6, 3),
            Salary = 100000.0M
        }
    };

        _employeeMockService.Setup(x => x.GetSortedFilteredEmployeesAsync(sortOrder, sortField, searchString))
            .ReturnsAsync(expectedEmployees);
        // Act
        var result = await _employeeController.Index(sortOrder, sortField, searchString, 1);
        var result2 = await _employeeController.Index(sortOrder, sortField, searchString, 1) as ViewResult;

        // Assert
        Assert.IsInstanceOfType(result, typeof(ViewResult));
        Assert.IsInstanceOfType(result2.Model, typeof(IPagedList<EmployeeViewModel>));
        var model = result2.Model as IPagedList<EmployeeViewModel>;
        Assert.AreEqual(expectedEmployees.ToList().Count, model.ToList().Count);

    }

    [TestMethod]
    public async Task Test_Index_ReturnsNotFoundForNullResult()
    {
        // Arrange
        string sortOrder = null;
        string sortField = null;
        string searchString = null;
        _employeeMockService.Setup(x => x.GetSortedFilteredEmployeesAsync(sortOrder, sortField, searchString))
            .ReturnsAsync((IEnumerable<EmployeeDto>)null);

        // Act
        var result = await _employeeController.Index(sortOrder, sortField, searchString, 1);

        // Assert
        Assert.IsInstanceOfType(result, typeof(NotFoundResult));
    }
    #endregion
}
