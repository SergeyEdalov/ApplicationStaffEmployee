using Autofac.Extensions.DependencyInjection;
using Autofac;
using AppStaffEmployee.Models.Database;
using AppStaffEmployee.Services;
using AppStaffEmployee.Services.Interfaces;
using AppStaffEmployee.Models.Dto;
using AppStaffEmployee.Models.Mapper;
using Identity.DAL.IdentityDB;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Identity.DAL.Entities;

namespace AppStaffEmployee;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        #region Конфигурация сервисов приложения

        builder.Services.AddControllersWithViews();
        builder.Services.AddAutoMapper(typeof(EmployeeMapper));
        builder.Services.AddIdentity<User, Role>()
            .AddEntityFrameworkStores<IdentityContext>()
            .AddDefaultTokenProviders();

        builder.Services.Configure<IdentityOptions>(opt =>
        {
            #if DEBUG
            opt.Password.RequireDigit = false;
            opt.Password.RequireLowercase = false;
            opt.Password.RequireUppercase = false;
            opt.Password.RequireNonAlphanumeric = false;
            opt.Password.RequiredLength = 3;
            opt.Password.RequiredUniqueChars = 3;
            #endif
            opt.User.RequireUniqueEmail = false;

            opt.Lockout.AllowedForNewUsers = false;
            opt.Lockout.MaxFailedAccessAttempts = 4;
            opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
        });

        builder.Services.ConfigureApplicationCookie(opt =>
        {
            opt.Cookie.Name = "AppStaffEmployee";
            opt.Cookie.HttpOnly = true;
            opt.ExpireTimeSpan = TimeSpan.FromDays(14);

            opt.LoginPath = "/Account/Login";
            opt.LogoutPath = "/Account/Logout";
            opt.AccessDeniedPath = "/Account/AccessDenied";

            opt.SlidingExpiration = true;
        });

        var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

        builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
        builder.Host.ConfigureContainer<ContainerBuilder>(cb =>
        {
            cb.Register(c => new EmployeeContext(config.GetConnectionString("employeeDb"))).InstancePerDependency();
            cb.Register(c =>
            {
                var optionsBuilder = new DbContextOptionsBuilder<IdentityContext>();
                optionsBuilder.UseNpgsql(config.GetConnectionString("employeeDb"));

                return new IdentityContext(optionsBuilder.Options);
            }).InstancePerDependency();
        });
        builder.Services.AddSingleton<IEmployeeService<EmployeeDto, Guid>, EmployeeService>();
        builder.Services.AddSingleton<IAccountService, AccountService>();

        #if RELEASE
        builder.WebHost.ConfigureKestrel((context, options) =>
        {
            options.ListenAnyIP(7129, listenOptions =>
            {
                listenOptions.UseHttps("/app/aspnetapp.pfx", "Str0ngP@ssw0rd!");
            });
            Debug.Print("!!!");
        });
        #endif

        #endregion

        var app = builder.Build();

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }
}
