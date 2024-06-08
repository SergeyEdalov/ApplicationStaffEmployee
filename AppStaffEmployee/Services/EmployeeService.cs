using AppStaffEmployee.Models;
using AppStaffEmployee.Models.Database;
using AppStaffEmployee.Models.Dto;
using AppStaffEmployee.Services.Interfaces;
using AppStaffEmployee.ViewModels;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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
        var employeeDto = _employeeMapper.Map<EmployeeDto>(employee);
        return employeeDto;
    }
    public async Task<Guid> AddEmployeeAsync(EmployeeDto? employeeData)
    {
        await _employeeContext.AddAsync(_employeeMapper.Map<EmployeeModel>(employeeData));
        await _employeeContext.SaveChangesAsync();
        _logger.LogInformation("Добавлен новый сотрудник {0}", employeeData.FullName);
        return (Guid)employeeData.Id;
    }

    public async Task<bool> EditEmployeeAsync(EmployeeDto employeeData)
    {
        var targetEmployee = await _employeeContext.Employees.FirstOrDefaultAsync(x => x.Id == employeeData.Id);

        if (targetEmployee is null) return false;

        targetEmployee = _employeeMapper.Map<EmployeeModel>(employeeData);
        _employeeContext.Employees.Update(targetEmployee);

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
        var employeesQuery = _employeeContext.Employees.AsNoTracking().AsQueryable();
        var employees = await employeesQuery.ToListAsync();

        if (!string.IsNullOrEmpty(searchString))
        {
            employees = employees.Where(e =>
                e.FullName.Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
                e.Birthday.ToString("dd.MM.yyyy").Contains(searchString) ||
                e.Department.Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
                e.JobTitle.Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
                e.WorkStart.ToString("dd.MM.yyyy").Contains(searchString) ||
                e.Salary.ToString().Contains(searchString))
                .ToList();
        }
        var employeeDtos = employees.Select(e => _employeeMapper.Map<EmployeeDto>(e));

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
        return employeeDtos.ToList();
    }
}