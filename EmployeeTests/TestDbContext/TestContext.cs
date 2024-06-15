using AppStaffEmployee.Models.Database;
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
        return context;
    }
}
