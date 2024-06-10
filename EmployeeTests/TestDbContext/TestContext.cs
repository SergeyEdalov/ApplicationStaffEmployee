using AppStaffEmployee.Models;
using AppStaffEmployee.Models.Database;
using Identity.DAL.IdentityDB;
using Microsoft.EntityFrameworkCore;

namespace EmployeeTests.TestDbContext;

public class TestDbContext
{
    public TestDbContext() { }
    public static EmployeeContext CreateTableEmployees()
    {
        var builder = new DbContextOptionsBuilder<EmployeeContext>()
            .UseInMemoryDatabase("EmployeeTestDbContext");

        var options = builder.Options;

        var context = new EmployeeContext(options);

        context.Database.EnsureCreated();
        //context.SaveChanges();
        return context;
    }

    public static IdentityContext CreateTableUsers()
    {
        var builder = new DbContextOptionsBuilder<IdentityContext>()
            .UseInMemoryDatabase("MessageTestDbContext");

        var options = builder.Options;

        var context = new IdentityContext(options);

        context.Database.EnsureCreated();

        return context;
    }
}
