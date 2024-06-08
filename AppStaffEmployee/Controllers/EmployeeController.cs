using AppStaffEmployee.Models.Dto;
using AppStaffEmployee.Services.Interfaces;
using AppStaffEmployee.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Printing;
using X.PagedList;

namespace ApplicationStaffEmployee.Controllers;

[Authorize]
public class EmployeeController : Controller
{
    private readonly IEmployeeService<EmployeeDto, Guid> _employeeService;
    private readonly ILogger<EmployeeController> _logger;
    private readonly IMapper _mapper;
    private const int _pageSize = 5;

    public EmployeeController(IEmployeeService<EmployeeDto, Guid> employeeService, ILogger<EmployeeController> logger, IMapper mapper)
    {
        _employeeService = employeeService;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<IActionResult> Index(string sortOrder, string sortField, string searchString, int? page)
    {
        int pageNumber = page ?? 1;
        var employeesTable = await _employeeService.GetSortedFilteredEmployeesAsync(sortOrder, sortField, searchString);
        var employeeView = employeesTable.Select(_mapper.Map<EmployeeViewModel>);
        ViewData["CurrentSortOrder"] = sortOrder;
        ViewData["CurrentSortField"] = sortField;
        ViewData["CurrentFilter"] = searchString;
        var result = employeeView.ToPagedList(pageNumber, _pageSize); // Для теста
        return View(result);
    }

    public async Task<IActionResult> Details (Guid id)
    {
        var employee = await _employeeService.GetEmpoloyeeByIDAsync(id);
        if (employee is null)  return NotFound();

        var employeeView = _mapper.Map<EmployeeViewModel>(employee);
        return View(employeeView);
    }

    public IActionResult Create()
    {
        return View("Create", new EmployeeViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> Create(EmployeeViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var employeeDto = _mapper.Map<EmployeeDto>(model);
        var id = await _employeeService.AddEmployeeAsync(employeeDto);
        
        return RedirectToAction("Details", new { id });
    }

    public async Task<IActionResult> Edit(Guid? id)
    {
        var employee = await _employeeService.GetEmpoloyeeByIDAsync((Guid)id);
        if (employee is null) return NotFound();

        var employeeView = _mapper.Map<EmployeeViewModel>(employee);

        return View(employeeView);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(EmployeeViewModel model)
    {
        if (!ModelState.IsValid) return View(model); 

        var employeeDto = _mapper.Map<EmployeeDto>(model);

        var success = await _employeeService.EditEmployeeAsync(employeeDto);
        
        if(!success) return NotFound();
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Delete(Guid id)
    {
        var employee = await _employeeService.GetEmpoloyeeByIDAsync(id);
        if (employee is null) return NotFound();

        var employeeView = _mapper.Map<EmployeeViewModel>(employee);
        return View(employeeView);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var success = await _employeeService.RemoveEmployeeAsync(id);
        if (!success) return NotFound();

        return RedirectToAction("Index");
    }
}
