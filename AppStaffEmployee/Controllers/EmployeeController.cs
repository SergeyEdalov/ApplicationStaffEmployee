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
        
        if (employeesTable is null) return NotFound();
        
        var employeeView = employeesTable.Select(_mapper.Map<EmployeeViewModel>);

        ViewData["CurrentSortOrder"] = sortOrder;
        ViewData["CurrentSortField"] = sortField;
        ViewData["CurrentFilter"] = searchString;

        var result = employeeView.ToPagedList(pageNumber, _pageSize); // Для теста
        _logger.LogInformation("Контроллер получил список всех сотрудников с сортировкой и фильтром");
        return View(result);
    }

    public async Task<IActionResult> Details(Guid id)
    {
        var employee = await _employeeService.GetEmpoloyeeByIDAsync(id);
        if (employee is null) return NotFound();

        var employeeView = _mapper.Map<EmployeeViewModel>(employee);
        _logger.LogInformation("Получена информация о сотруднике {0}", employeeView.FullName);
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
        try
        {
            var employeeDto = _mapper.Map<EmployeeDto>(model);
            var id = await _employeeService.AddEmployeeAsync(employeeDto);

            _logger.LogInformation("Добавлен новый сотрудник {0}. Переход на страницу \"Детали\" ", employeeDto.FullName);
            return RedirectToAction("Details", new { id });
        }
        catch (Exception ex) 
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            _logger.LogError("Ошибка добавления сотрудника {0}.", model.FullName);
            return View(model);
        }

    }

    public async Task<IActionResult> Edit(Guid? id)
    {
        var employee = await _employeeService.GetEmpoloyeeByIDAsync((Guid)id);
        if (employee is null) return NotFound();

        var employeeView = _mapper.Map<EmployeeViewModel>(employee);

        _logger.LogInformation("Получен сотрудник {0} для редактирования.", employeeView.FullName);

        return View(employeeView);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(EmployeeViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var employeeDto = _mapper.Map<EmployeeDto>(model);

        var success = await _employeeService.EditEmployeeAsync(employeeDto);

        if (!success) return NotFound();

        _logger.LogInformation("Сотрудник {0} отредактирован.", employeeDto.FullName);
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Delete(Guid id)
    {
        var employee = await _employeeService.GetEmpoloyeeByIDAsync(id);
        if (employee is null) return NotFound();

        var employeeView = _mapper.Map<EmployeeViewModel>(employee);

        _logger.LogInformation("Получен сотрудник {0} для удаления.", employeeView.FullName);
        //return View(employeeView); //Вариант для перехода на отдельную страницу
        return PartialView("DeleteEmployeeModalWindow", employeeView); //Вариант для перехода на модальное окно
    }

    [HttpPost]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var success = await _employeeService.RemoveEmployeeAsync(id);
        if (!success) return NotFound();

        _logger.LogInformation("Сотрудник {0} удален из списка.", id);

        return RedirectToAction("Index");
    }
}
