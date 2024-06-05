using AppStaffEmployee.ViewModels.Identity;
using Identity.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AppStaffEmployee.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly ILogger<AccountController> _logger;

    public AccountController(
        UserManager<User> userManager, 
        SignInManager<User> signInManager, ILogger<AccountController> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _logger = logger;
    }

    public IActionResult Register()
    {
        return View(new RegisterUserViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterUserViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var user = new User { UserName = model.UserName };

        var result = await _userManager.CreateAsync(user, model.Password);

        if (result.Succeeded)
        {
            _logger.LogInformation("Пользователь {0} успешно создан", user);

            await _signInManager.SignInAsync(user, isPersistent: false);

            return RedirectToAction("Index", "Home");
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError("", error.Description);   
        }
        return View(model);
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
