using AppStaffEmployee.Models.Dto;
using AppStaffEmployee.Services.Interfaces;
using AppStaffEmployee.ViewModels.Identity;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AppStaffEmployee.Controllers;

public class AccountController : Controller
{
    private readonly IAccountService _accountService;
    private readonly IMapper _mapper;
    private readonly ILogger<AccountController> _logger;

    public AccountController(IAccountService accountService,
        IMapper mapper, ILogger<AccountController> logger)
    {
        _accountService = accountService;
        _mapper = mapper;
        _logger = logger;
    }

    #region Регистрация нового пользователя
    public IActionResult Register()
    {
        return View(new RegisterUserViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterUserViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var userDto = _mapper.Map<UserDto>(model);

        var result = await _accountService.RegisterAsync(userDto);

        if (result.Succeeded)
            return RedirectToAction("Index", "Home");

        foreach (var error in result.Errors)
        {
            _logger.LogError(error.Description);
            ModelState.AddModelError("", error.Description);
        }
        return View(model);
    }
    #endregion


    #region Вход в систему
    public IActionResult Login(string? ReturnUrl = null)
    {
        return View(new LoginViewModel
        {
            ReturnUrl = ReturnUrl
        });
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var userDto = _mapper.Map<UserDto>(model);

        var result = await _accountService.LoginAsync(userDto);

        if (result.Succeeded)
        {
            _logger.LogInformation("Пользователь {0} успешно вошел в систему", model.UserName);

            return LocalRedirect(model.ReturnUrl ?? "/");
        }

        ModelState.AddModelError("", "Неверное имя пользователя или пароль");
        _logger.LogWarning("Ошибка при входе в систему {0}", model.UserName);

        return View(model);
    }
    #endregion
    public async Task<IActionResult> Logout()
    {
        await _accountService.LogoutAsync();
        return RedirectToAction("Index", "Home");
    }
    public async Task<IActionResult> AccessDenied() => View();
}
