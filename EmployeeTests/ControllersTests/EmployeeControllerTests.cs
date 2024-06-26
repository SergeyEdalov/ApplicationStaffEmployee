﻿using ApplicationStaffEmployee.Controllers;
using AppStaffEmployee.Models.Dto;
using AppStaffEmployee.Services.Interfaces;
using AppStaffEmployee.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using X.PagedList;

namespace EmployeeTests.ControllersTests;

[TestClass]
public class EmployeeControllerTests
{
    private static Mock<IMapper> _employeeMockMapper;
    private static Mock<ILogger<EmployeeController>> _loggerMock;
    private static Mock<IEmployeeService<EmployeeDto, Guid>> _employeeMockService;
    private EmployeeController _employeeController;

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
    }

    [TestInitialize]
    public void Start()
    {
        _employeeController = new EmployeeController(_employeeMockService.Object, _loggerMock.Object, _employeeMockMapper.Object);
    }
    #endregion

    #region Тесты метода Index 
    [TestMethod]
    public async Task Test_Index_Success_ReturnEmployeeViewModel()
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
        var model = result2.Model as IPagedList<EmployeeViewModel>;

        // Assert
        Assert.IsInstanceOfType(result, typeof(ViewResult));
        Assert.IsInstanceOfType(result2.Model, typeof(IPagedList<EmployeeViewModel>));

        Assert.AreEqual(expectedEmployees.Count, model.Count);
        Assert.AreEqual(expectedEmployees.Select(x => x.FullName).First(), model.Select(x => x.FullName).First());
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

    #region Тесты метода Details 
    [TestMethod]
    public async Task Test_Details_Success_ReturnEmployeeViewModel()
    {
        // Arrange
        var expectedEmployee = new EmployeeDto()
        {
            Id = Guid.Parse("bc3d5e78-fe17-4e95-b08e-a56d58384325"),
            FullName = "Popov Ivan Nikolaevich",
            Birthday = new DateTime(1985, 2, 5),
            Department = "marketing",
            JobTitle = "trainee",
            WorkStart = new DateTime(2023, 9, 16),
            Salary = 56000.0M
        };
        _employeeMockService.Setup(x => x.GetEmpoloyeeByIDAsync((Guid)expectedEmployee.Id))
            .ReturnsAsync(expectedEmployee);

        // Act
        var result = await _employeeController.Details((Guid)expectedEmployee.Id) as ViewResult;
        var model = result.Model as EmployeeViewModel;

        // Assert
        Assert.IsInstanceOfType(result, typeof(ViewResult));
        Assert.AreEqual(expectedEmployee.FullName, model.FullName);
    }

    [TestMethod]
    public async Task Test_Details_ReturnNotFoundEmployee()
    {
        // Arrange
        var employeeId = Guid.NewGuid();
        _employeeMockService.Setup(x => x.GetEmpoloyeeByIDAsync(employeeId))
            .ReturnsAsync((EmployeeDto)null);

        // Act
        var result = await _employeeController.Details(employeeId);

        // Assert
        Assert.IsInstanceOfType(result, typeof(NotFoundResult));
    }
    #endregion

    #region Тесты метода Create 
    [TestMethod]
    public async Task Test_CreatePost_Success_ReturnEmployeeId()
    {
        // Arrange
        var model = new EmployeeViewModel
        {
            Id = Guid.NewGuid(),
            FullName = "Test Employee",
            Birthday = new DateTime(1985, 1, 1),
            Department = "Test Department",
            JobTitle = "Test Job Title",
            WorkStart = new DateTime(2000, 1, 1),
            Salary = 50000.0M
        };
        var employeeDto = new EmployeeDto
        {
            Id = model.Id,
            FullName = model.FullName,
            Birthday = model.Birthday,
            Department = model.Department,
            JobTitle = model.JobTitle,
            WorkStart = model.WorkStart,
            Salary = model.Salary
        };
        _employeeMockService.Setup(s => s.AddEmployeeAsync(It.IsAny<EmployeeDto>())).ReturnsAsync((Guid)employeeDto.Id);

        // Act
        var result = await _employeeController.Create(model);
        var redirectResult = result as RedirectToActionResult;

        // Assert
        Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
        Assert.AreEqual("Details", redirectResult.ActionName);
        Assert.AreEqual(employeeDto.Id, redirectResult.RouteValues["id"]);
    }

    [TestMethod]
    public async Task Test_CreatePost_InvalidModelState()
    {
        // Arrange
        var model = new EmployeeViewModel();
        _employeeController.ModelState.AddModelError("Name", "Required");

        // Act
        var result = await _employeeController.Create(model);
        var viewResult = result as ViewResult;

        // Assert
        Assert.IsInstanceOfType(result, typeof(ViewResult));
        Assert.AreEqual(viewResult?.Model, model);
    }

    [TestMethod]
    public async Task Test_CreatePost_Exception()
    {
        // Arrange
        var model = new EmployeeViewModel
        {
            FullName = "Test Employee",
            Birthday = new DateTime(1980, 1, 1),
            Department = "Test Department",
            JobTitle = "Test Job Title",
            WorkStart = new DateTime(2000, 1, 1),
            Salary = 50000.0M
        };
        var expectedException = new Exception("Test Exception");

        _employeeMockService.Setup(s => s.AddEmployeeAsync(It.IsAny<EmployeeDto>())).ThrowsAsync(expectedException);

        // Act
        var result = await _employeeController.Create(model);
        var viewResult = result as ViewResult;

        // Assert
        Assert.IsInstanceOfType(result, typeof(ViewResult));
        Assert.IsNotNull(viewResult);
        Assert.IsTrue(viewResult.ViewData.ModelState.ContainsKey(string.Empty));
        Assert.AreEqual(expectedException.Message, viewResult.ViewData.ModelState[string.Empty]?.Errors[0].ErrorMessage);
    }
    #endregion

    #region Тесты метода Edit 
    [TestMethod]
    public async Task Test_Edit_Success_ReturnEmployeeView()
    {
        // Arrange
        var employeeId = new Guid("db51e04d-e114-4e7f-bfe2-87ab10a48851");
        var employeeDto = new EmployeeDto
        {
            Id = employeeId,
            FullName = "Test Employee",
            Birthday = new DateTime(1980, 1, 1),
            Department = "Test Department",
            JobTitle = "Test Job Title",
            WorkStart = new DateTime(2000, 1, 1),
            Salary = 50000.0M
        };
        _employeeMockService.Setup(x => x.GetEmpoloyeeByIDAsync(employeeId)).ReturnsAsync(employeeDto);

        // Act
        var result = await _employeeController.Edit(employeeId);
        var viewResult = result as ViewResult;
        var model = viewResult?.Model as EmployeeViewModel;

        // Assert
        Assert.IsInstanceOfType(result, typeof(ViewResult));
        Assert.IsNotNull(viewResult);
        Assert.AreEqual(employeeId, model?.Id);
    }

    [TestMethod]
    public async Task Test_Edit_Error_EmployeeNotFound()
    {
        // Arrange
        var employeeId = new Guid("db51e04d-e114-4e7f-bfe2-87ab10a48851");
        _employeeMockService.Setup(x => x.GetEmpoloyeeByIDAsync(employeeId)).ReturnsAsync((EmployeeDto?)null);

        // Act
        var result = await _employeeController.Edit(employeeId);

        // Assert
        Assert.IsInstanceOfType(result, typeof(NotFoundResult));
    }

    [TestMethod]
    public async Task Test_EditPost_Success()
    {
        // Arrange
        var model = new EmployeeViewModel
        {
            Id = new Guid("db51e04d-e114-4e7f-bfe2-87ab10a48851"),
            FullName = "Test Employee",
            Birthday = new DateTime(1980, 1, 1),
            Department = "Test Department",
            JobTitle = "Test Job Title",
            WorkStart = new DateTime(2000, 1, 1),
            Salary = 50000.0M
        };
        _employeeMockService.Setup(x => x.EditEmployeeAsync(It.IsAny<EmployeeDto>())).ReturnsAsync(true);

        // Act
        var result = await _employeeController.Edit(model);
        var redirectResult = result as RedirectToActionResult;

        // Assert
        Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
        Assert.AreEqual("Index", redirectResult.ActionName);
    }

    [TestMethod]
    public async Task Test_EditPost_InvalidModelState()
    {
        // Arrange
        var model = new EmployeeViewModel();
        _employeeController.ModelState.AddModelError("Name", "Required");

        // Act
        var result = await _employeeController.Edit(model);
        var viewResult = result as ViewResult;

        // Assert
        Assert.IsInstanceOfType(result, typeof(ViewResult));
        Assert.AreEqual(model, viewResult.Model);
    }

    [TestMethod]
    public async Task Test_EditPost_EditFailed()
    {
        // Arrange
        var model = new EmployeeViewModel
        {
            Id = new Guid("db51e04d-e114-4e7f-bfe2-87ab10a48851"),
            FullName = "Test Employee",
            Birthday = new DateTime(1980, 1, 1),
            Department = "Test Department",
            JobTitle = "Test Job Title",
            WorkStart = new DateTime(2000, 1, 1),
            Salary = 50000.0M
        };
        _employeeMockService.Setup(x => x.EditEmployeeAsync(It.IsAny<EmployeeDto>())).ReturnsAsync(false);

        // Act
        var result = await _employeeController.Edit(model);

        // Assert
        Assert.IsInstanceOfType(result, typeof(NotFoundResult));
    }

    #endregion

    #region Тесты метода Delete 
    [TestMethod]
    public async Task Test_Delete_Success_ReturnEmployeePartialView()
    {
        // Arrange
        var employeeId = new Guid("db51e04d-e114-4e7f-bfe2-87ab10a48851");
        var employeeDto = new EmployeeDto
        {
            Id = employeeId,
            FullName = "Test Employee",
            Birthday = new DateTime(1980, 1, 1),
            Department = "Test Department",
            JobTitle = "Test Job Title",
            WorkStart = new DateTime(2000, 1, 1),
            Salary = 50000.0M
        };
        _employeeMockService.Setup(x => x.GetEmpoloyeeByIDAsync(employeeId)).ReturnsAsync(employeeDto);

        // Act
        var result = await _employeeController.Delete(employeeId);
        var partialViewREsult = result as PartialViewResult;
        var model = partialViewREsult?.Model as EmployeeViewModel;

        // Assert
        Assert.IsInstanceOfType(result, typeof(PartialViewResult));
        Assert.IsNotNull(partialViewREsult);
        Assert.AreEqual(employeeId, model?.Id);
    }

    [TestMethod]
    public async Task Test_Delete_Error_EmployeeNotFound()
    {
        // Arrange
        var employeeId = new Guid("db51e04d-e114-4e7f-bfe2-87ab10a48851");
        _employeeMockService.Setup(x => x.GetEmpoloyeeByIDAsync(employeeId)).ReturnsAsync((EmployeeDto)null);

        // Act
        var result = await _employeeController.Delete(employeeId);

        // Assert
        Assert.IsInstanceOfType(result, typeof(NotFoundResult));
    }

    [TestMethod]
    public async Task Test_DeleteConfirmed_Success_EmployeeRemove_Redirect()
    {
        // Arrange
        var employeeId = new Guid("db51e04d-e114-4e7f-bfe2-87ab10a48851");
        _employeeMockService.Setup(x => x.RemoveEmployeeAsync(employeeId)).ReturnsAsync(true);

        // Act
        var result = await _employeeController.DeleteConfirmed(employeeId);
        var redirectResult = result as RedirectToActionResult;

        // Assert
        Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
        Assert.IsNotNull(redirectResult);
        Assert.AreEqual("Index", redirectResult.ActionName);
    }

    [TestMethod]
    public async Task Test_DeleteConfirmed_Error_EmployeeNotFound()
    {
        // Arrange
        var employeeId = new Guid("db51e04d-e114-4e7f-bfe2-87ab10a48851");
        _employeeMockService.Setup(x => x.RemoveEmployeeAsync(employeeId)).ReturnsAsync(false);

        // Act
        var result = await _employeeController.DeleteConfirmed(employeeId);

        // Assert
        Assert.IsInstanceOfType(result, typeof(NotFoundResult));
    }
    #endregion
}
