namespace AppStaffEmployee.Services;

    public interface IEmployeeService <T, E>
    {
        Task<IEnumerable<T>> GetAllEmployeesAsync();
        Task<T?> GetEmpoloyeeByIDAsync(E employeeId);
        Task<E> AddEmployeeAsync(T? employeeData);
        Task<bool> EditEmployeeAsync(E? employeeId, T employeeData);
        Task<bool> RemoveEmployeeAsync(E? employeeId);
    }

