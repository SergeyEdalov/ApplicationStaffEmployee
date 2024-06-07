using AppStaffEmployee.Models.Dto;
using Microsoft.AspNetCore.Identity;

namespace AppStaffEmployee.Services.Interfaces;

public interface IAccountService
{
    public Task<IdentityResult> RegisterAsync(UserDto userDto);
    public Task<SignInResult> LoginAsync(UserDto user);
    public Task LogoutAsync();
}