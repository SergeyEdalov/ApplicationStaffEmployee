using AppStaffEmployee.Models.Database;
using Identity.DAL.IdentityDB;
using Microsoft.EntityFrameworkCore;

namespace EmployeeTests.TestDbContext;

public class TestCommandBase
{
    public EmployeeContext _employeeContext;
    public IdentityContext _identityContext;

    public static EmployeeContext getEContext() => TestDbContext.CreateTableEmployees();
    
    public static IdentityContext getUContext() => TestDbContext.CreateTableUsers();

    public static void CleanEmployeesContext(EmployeeContext context)
    {
        context.Employees.RemoveRange(context.Employees.Select(x => x));
        context.SaveChanges();
    }
    public static void CleanUserContext(IdentityContext context)
    {
        context.Users.RemoveRange(context.Users.Select(x => x));
        context.SaveChanges();
    }
    public static void Destroy(DbContext context)
    {
        context.Dispose();
    }
}
