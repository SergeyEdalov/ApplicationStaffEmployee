using AppStaffEmployee.Models;
using AppStaffEmployee.Models.Dto;
using AppStaffEmployee.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AppStaffEmployee.Controllers;

public class HomeController : Controller
{
    private readonly IEmployeeService<EmployeeDto, Guid> _employeeService;
    private readonly ILogger<HomeController> _logger;

    public HomeController(IEmployeeService<EmployeeDto, Guid> employeeService, ILogger<HomeController> logger)
    {
        _employeeService = employeeService;
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        var employeesTable = await _employeeService.GetAllEmployeesAsync();
        return View(employeesTable);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

