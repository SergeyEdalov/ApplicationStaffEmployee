using AppStaffEmployee.Models;
using AppStaffEmployee.Models.Database;
using AppStaffEmployee.Models.Dto;
using AppStaffEmployee.Services.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace AppStaffEmployee.Services;

public class EmployeeService : IEmployeeService<EmployeeDto, Guid>
{
    private readonly EmployeeContext _employeeContext;
    private readonly IMapper _employeeMapper;
    private readonly ILogger<EmployeeService> _logger;

    public EmployeeService(EmployeeContext employeeContext, IMapper employeeMapper, ILogger<EmployeeService> logger)
    {
        _employeeContext = employeeContext;
        _employeeMapper = employeeMapper;
        _logger = logger;
    }

    public async Task<IEnumerable<EmployeeDto>> GetAllEmployeesAsync()
    {
        var employeeList = await _employeeContext.Employees.Select(x => _employeeMapper.Map<EmployeeDto>(x)).ToListAsync();
        _logger.LogInformation("Получен список всех сотрудников");

        return employeeList;
    }

    public async Task<EmployeeDto?> GetEmpoloyeeByIDAsync(Guid employeeId)
    {
        var employee = await _employeeContext.Employees.FirstOrDefaultAsync(x => x.Id.Equals(employeeId));

        if (employee is null)
        {
            _logger.LogWarning($"Не найден сотрудник в БД по id = {0}", employeeId);
            return null;
            //throw new NullReferenceException($"{employeeId} нет в базе данных");
        }

        var employeeDto = _employeeMapper.Map<EmployeeDto>(employee);
        _logger.LogInformation("Получен id сотрудника {0}", employee.FullName);
        return employeeDto;
    }
    public async Task<Guid> AddEmployeeAsync(EmployeeDto? employeeData)
    {
        employeeData.Id = Guid.NewGuid();
        try
        {
            var employeeModel = _employeeMapper.Map<EmployeeModel>(employeeData);
            await _employeeContext.AddAsync(employeeModel);
            await _employeeContext.SaveChangesAsync();
            _logger.LogInformation("Добавлен новый сотрудник {0}", employeeData.FullName);
            return (Guid)employeeData.Id;
        }
        catch (Exception ex) { throw new Exception("Ошибка добавления сотрудника"); }
    }

    public async Task<bool> EditEmployeeAsync(EmployeeDto employeeData)
    {
        var targetEmployee = await _employeeContext.Employees.FirstOrDefaultAsync(x => x.Id == employeeData.Id);

        if (targetEmployee is null) return false;
        _employeeMapper.Map(employeeData, targetEmployee);

        await _employeeContext.SaveChangesAsync();
        _logger.LogInformation("Сотрудник {0} отредактирован", employeeData.FullName);
        return true;
    }

    public async Task<bool> RemoveEmployeeAsync(Guid employeeId)
    {
        var targetEmployee = await _employeeContext.Employees.FirstOrDefaultAsync(x => x.Id == employeeId);

        if (targetEmployee is null) return false;

        _employeeContext.Employees.Remove(targetEmployee);
        await _employeeContext.SaveChangesAsync();
        _logger.LogInformation("Сотрудник {0} удален", targetEmployee.FullName);

        return true;
    }


    public async Task<IEnumerable<EmployeeDto>> GetSortedFilteredEmployeesAsync(string sortOrder, string sortField, string searchString)
    {
        var employeesQuery = await _employeeContext.Employees.AsNoTracking().AsQueryable().ToListAsync();
        //var employees = await employeesQuery.ToListAsync();

        if (!string.IsNullOrEmpty(searchString))
        {
            employeesQuery = employeesQuery.Where(e =>
                e.FullName.Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
                e.Birthday.ToString("dd.MM.yyyy").Contains(searchString) ||
                e.Department.Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
                e.JobTitle.Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
                e.WorkStart.ToString("dd.MM.yyyy").Contains(searchString) ||
                e.Salary.ToString().Contains(searchString))
                .ToList();
        }
        var employeeDtos = employeesQuery.Select(e => _employeeMapper.Map<EmployeeDto>(e));

        if (!string.IsNullOrEmpty(sortField) && !string.IsNullOrEmpty(sortOrder))
        {
            switch (sortField)
            {
                case "FullName":
                    employeeDtos = sortOrder == "asc" ? employeeDtos.OrderBy(e => e.FullName) : employeeDtos.OrderByDescending(e => e.FullName);
                    break;
                case "Birthday":
                    employeeDtos = sortOrder == "asc" ? employeeDtos.OrderBy(e => e.Birthday) : employeeDtos.OrderByDescending(e => e.Birthday);
                    break;
                case "Department":
                    employeeDtos = sortOrder == "asc" ? employeeDtos.OrderBy(e => e.Department) : employeeDtos.OrderByDescending(e => e.Department);
                    break;
                case "JobTitle":
                    employeeDtos = sortOrder == "asc" ? employeeDtos.OrderBy(e => e.JobTitle) : employeeDtos.OrderByDescending(e => e.JobTitle);
                    break;
                case "WorkStart":
                    employeeDtos = sortOrder == "asc" ? employeeDtos.OrderBy(e => e.WorkStart) : employeeDtos.OrderByDescending(e => e.WorkStart);
                    break;
                case "Salary":
                    employeeDtos = sortOrder == "asc" ? employeeDtos.OrderBy(e => e.Salary) : employeeDtos.OrderByDescending(e => e.Salary);
                    break;
                default:
                    employeeDtos = employeeDtos.OrderBy(e => e.FullName);
                    break;
            }
        }
        var result = employeeDtos; // Для теста
        return employeeDtos;
    }
}