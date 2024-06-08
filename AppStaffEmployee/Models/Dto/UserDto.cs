﻿using Microsoft.AspNetCore.Mvc;

namespace AppStaffEmployee.Models.Dto
{
    public class UserDto
    {
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
        public bool RememberMe { get; set; }
        public string? ReturnUrl { get; set; }
    }
}