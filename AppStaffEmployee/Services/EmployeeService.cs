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
        // Логика выполнения запроса к базе данных с учетом сортировки и фильтрации
        //var query = _employeeContext.Employees.AsQueryable();
        //var query = (IEnumerable<EmployeeDto>) await _employeeContext.Employees
        //    .Select(x => _employeeMapper.Map<EmployeeDto>(x)).ToListAsync();
        
        var employeesQuery = _employeeContext.Employees.AsQueryable();

        if (!string.IsNullOrEmpty(searchString))
        {
            employeesQuery = employeesQuery.Where(e => e.FullName.Contains(searchString));
        }
        var employees = await employeesQuery.AsNoTracking().ToListAsync();
        var employeeDtos = employees.Select(e => _employeeMapper.Map<EmployeeDto>(e));

        if (!string.IsNullOrEmpty(sortField) && !string.IsNullOrEmpty(sortOrder))
        {
            switch (sortField)
            {
                case "FullName":
                    employeeDtos = sortOrder == "asc" ? employeeDtos.OrderBy(e => e.FullName) : employeeDtos.OrderByDescending(e => e.FullName);
                    break;
                case "Salary":
                    employeeDtos = sortOrder == "asc" ? employeeDtos.OrderBy(e => e.Salary) : employeeDtos.OrderByDescending(e => e.Salary);
                    break;
                // Add more fields as needed
                default:
                    employeeDtos = employeeDtos.OrderBy(e => e.FullName);
                    break;
            }
        }
        //var employeesDto = ForEach(e =>  _employeeMapper.Map<EmployeeDto>(e));
        //foreach (var item in employees)
        //{
        //    _employeeMapper.Map<EmployeeDto>(item);
        //}
        //var result = await employeesQuery.AsNoTracking().ToListAsync();
        return employeeDtos.ToList();
    }
}