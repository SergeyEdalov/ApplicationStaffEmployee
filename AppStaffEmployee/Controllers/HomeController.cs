using Microsoft.AspNetCore.Mvc;

namespace AppStaffEmployee.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        _logger.LogInformation("Выведена домашняя страница");
        return View();
    }
}

