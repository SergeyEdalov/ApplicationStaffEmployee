using AppStaffEmployee.Models.Database;
using Microsoft.EntityFrameworkCore;

namespace EmployeeTests.TestDbContext;

public class TestCommandBase
{
    public static EmployeeContext _employeeContext;

    public static EmployeeContext getEContext() => TestDbContext.CreateTableEmployees();
    
    public static void Destroy(DbContext context) => context.Dispose();
}
