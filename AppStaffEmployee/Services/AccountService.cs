using AppStaffEmployee.Models.Dto;
using AppStaffEmployee.Services.Interfaces;
using Identity.DAL.Entities;
using Microsoft.AspNetCore.Identity;

namespace AppStaffEmployee.Services;

public class AccountService : IAccountService
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly ILogger<AccountService> _logger;

    public AccountService(UserManager<User> userManager,
        SignInManager<User> signInManager, ILogger<AccountService> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _logger = logger;
    }

    #region Регистрация нового пользователя
    public async Task<IdentityResult> RegisterAsync(UserDto userDto)
    {
        var user = new User { UserName = userDto.UserName };

        var result = await _userManager.CreateAsync(user, userDto.Password);

        if (result.Succeeded)
        {
            _logger.LogInformation("Пользователь {0} успешно создан", user);
            await _signInManager.SignInAsync(user, isPersistent: false);
        }
        return result;
    }
    #endregion

    #region Вход в систему
    public async Task<SignInResult> LoginAsync(UserDto user)
    {
        var result = await _signInManager.PasswordSignInAsync(
            user.UserName,
            user.Password,
            user.RememberMe,
            true);

        if (result.Succeeded)
            _logger.LogInformation("Пользователь {0} успешно вошел в систему", user.UserName);

        return result;
    }
    #endregion
    public async Task LogoutAsync() => await _signInManager.SignOutAsync();
}
