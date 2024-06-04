﻿namespace AppStaffEmployee.Services.Interfaces;

public interface IEmployeeService<T, E>
{
    Task<IEnumerable<T>> GetAllEmployeesAsync();
    Task<T?> GetEmpoloyeeByIDAsync(E employeeId);
    Task<E> AddEmployeeAsync(T? employeeData);
    Task<bool> EditEmployeeAsync(T employeeData);
    Task<bool> RemoveEmployeeAsync(E? employeeId);
}
