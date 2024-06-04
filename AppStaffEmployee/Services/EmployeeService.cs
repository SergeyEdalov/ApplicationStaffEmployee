using AppStaffEmployee.Models;
using AppStaffEmployee.Models.Database;
using AppStaffEmployee.Models.Dto;
using AppStaffEmployee.Services.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

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
        //_logger.Log("Добавлен новый пользователь {0}", employeeData.FullName);
        return employeeData.Id;
    }

    public async Task<bool> EditEmployeeAsync(Guid employeeId, EmployeeDto employeeData)
    {
        var targetEmployee = await _employeeContext.Employees.FirstOrDefaultAsync(x => x.Id == employeeId);

        if (targetEmployee is null) return false;

        targetEmployee = _employeeMapper.Map<EmployeeModel>(employeeData);
        _employeeContext.Employees.Update(targetEmployee);

        await _employeeContext.SaveChangesAsync();
        return true;
    }


    public async Task<bool> RemoveEmployeeAsync(Guid employeeId)
    {
        var targetEmployee = await _employeeContext.Employees.FirstOrDefaultAsync(x => x.Id == employeeId);

        if (targetEmployee is null) return false;

        _employeeContext.Employees.Remove(targetEmployee);
        await _employeeContext.SaveChangesAsync();

        return true;
    }
}

