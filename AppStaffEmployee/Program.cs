using Autofac.Extensions.DependencyInjection;
using Autofac;
using AppStaffEmployee.Models.Database;
using AppStaffEmployee.Services;
using AppStaffEmployee.Services.Interfaces;
using AppStaffEmployee.Models.Dto;

namespace AppStaffEmployee
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
            builder.Host.ConfigureContainer<ContainerBuilder>(cb =>
            {
                cb.Register(c => new EmployeeContext(config.GetConnectionString("employeeDb"))).InstancePerDependency();
            });
            builder.Services.AddSingleton<IEmployeeService<EmployeeDto, Guid>, EmployeeService>();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            //app.UseAuthentication();    
            //app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
