using Identity.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AppStaffEmployee.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<User> _user;
    private readonly SignInManager<User> _signInManager;
    private readonly ILogger<AccountController> _logger;

    public AccountController(
        UserManager<User> user, 
        SignInManager<User> signInManager, ILogger<AccountController> logger)
    {
        _user = user;
        _signInManager = signInManager;
        _logger = logger;
    }

    public async Task<IActionResult> Register()
    {
        return View();
    }   
    public async Task<IActionResult> Login()
    {
        return View();
    }   
    public async Task<IActionResult> Logout()
    {
        return View();
    }   
    public async Task<IActionResult> AccessDenied()
    {
        return View();
    }


}
