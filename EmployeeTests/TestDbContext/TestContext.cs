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
        context.Employees.AddRange(
            new EmployeeModel
            {
                Id = Guid.Parse("bc3d5e78-fe17-4e95-b08e-a56d58384325"),
                FullName = "Popov Ivan Nikolaevich",
                Birthday = new DateTime(1985, 2, 5),
                Department = "marketing",
                JobTitle = "trainee",
                WorkStart = new DateTime(2023, 9, 16),
                Salary = 56000.0M
            },
            new EmployeeModel
            {
                Id = Guid.Parse("db51e04d-e114-4e7f-bfe2-87ab10a48bbf"),
                FullName = "Djekson Bob",
                Birthday = new DateTime(1996, 10, 10),
                Department = "development",
                JobTitle = "middle devops",
                WorkStart = new DateTime(2020, 12, 25),
                Salary = 80000.0M
            },
            new EmployeeModel
            {
                Id = Guid.Parse("67bbefef-a586-4416-b5f5-8d70f3b51d44"),
                FullName = "Bastriykina Maria Sergeevna",
                Birthday = new DateTime(1991, 1, 9),
                Department = "bookkeeping",
                JobTitle = "accountant",
                WorkStart = new DateTime(2015, 9, 22),
                Salary = 60000.0M
            },
            new EmployeeModel
            {
                Id = Guid.Parse("af742a65-2b76-4895-91cf-7e35019309fd"),
                FullName = "Csvetov Victor Andreevich",
                Birthday = new DateTime(1994, 9, 6),
                Department = "sale",
                JobTitle = "chief Sales Specialist",
                WorkStart = new DateTime(2017, 6, 5),
                Salary = 115000.0M
            },
            new EmployeeModel
            {
                Id = Guid.Parse("6afd4cd6-0fe0-4d0b-aa72-bd4bf97a4860"),
                FullName = "Konovalova Irina Alekseevna",
                Birthday = new DateTime(1978, 3, 30),
                Department = "bookkeeping",
                JobTitle = "chief accountant",
                WorkStart = new DateTime(2014, 6, 6),
                Salary = 90000.0M
            },
            new EmployeeModel
            {
                Id = Guid.Parse("a0cf8d7c-f8a1-460a-9b97-14a58fae574f"),
                FullName = "Ermakov Sergey Vasilevich",
                Birthday = new DateTime(1990, 5, 28),
                Department = "financial",
                JobTitle = "financial director",
                WorkStart = new DateTime(2022, 6, 3),
                Salary = 100000.0M
            });
        context.SaveChanges();
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
