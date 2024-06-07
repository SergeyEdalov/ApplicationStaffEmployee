using AppStaffEmployee.Models.Dto;
using AppStaffEmployee.Services.Interfaces;
using AppStaffEmployee.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationStaffEmployee.Controllers;

[Authorize]
public class EmployeeController : Controller
{
    private readonly IEmployeeService<EmployeeDto, Guid> _employeeService;
    private readonly ILogger<EmployeeController> _logger;
    private readonly IMapper _mapper;

    public EmployeeController(IEmployeeService<EmployeeDto, Guid> employeeService, ILogger<EmployeeController> logger, IMapper mapper)
    {
        _employeeService = employeeService;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<IActionResult> Index(string sortOrder, string sortField, string searchString)
    {
        //var employeesTable = await _employeeService.GetAllEmployeesAsync();
        //var employeesTable = await _employeeService.GetSortedAndFilteredDataAsync(sortBy, filterBy);
        var employeesTable = await _employeeService.GetSortedFilteredEmployeesAsync(sortOrder, sortField, searchString);
        var employeeView = employeesTable.Select(_mapper.Map<EmployeeViewModel>);
        return View(employeeView);
    }

    public async Task<IActionResult> Details (Guid id)
    {
        var employee = await _employeeService.GetEmpoloyeeByIDAsync(id);
        if (employee is null)  return NotFound();

        var employeeView = _mapper.Map<EmployeeViewModel>(employee);
        return View(employeeView);
    }

    public async Task<IActionResult> Create()
    {
        return View("Edit", new EmployeeViewModel());
    }

    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null) return View(new EmployeeViewModel());

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

        if(employeeDto.Id is null)
        {
            var id = await _employeeService.AddEmployeeAsync(employeeDto);
            return RedirectToAction("Details", new {id});
        }
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
