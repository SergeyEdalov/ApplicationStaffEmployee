using AppStaffEmployee.Models;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

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

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public async Task<IActionResult> Error()
    {
        var exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

        if (exceptionHandlerPathFeature?.Error != null)
            _logger.LogError("Произошла ошибка: {Error}", exceptionHandlerPathFeature.Error);

        var errorView = await Task.FromResult(View(new ErrorViewModel 
            { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier }));
        
        return errorView;
    }

    [Route("Home/Error/{statusCode}")]
    public async Task<IActionResult> HttpStatusCodeHandler(int statusCode)
    {
        var statusCodeResult = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();

        switch (statusCode)
        {
            case 404:
                ViewBag.ErrorMessage = "Извините, запрашиваемая страница не найдена";
                _logger.LogWarning($"404 Error occurred. Path = {statusCodeResult?.OriginalPath} and QueryString = {statusCodeResult?.OriginalQueryString}");
                break;
            default:
                ViewBag.ErrorMessage = "Произошла ошибка";
                _logger.LogError($"Error occurred. Status code = {statusCode}");
                break;
        }
        return await Task.FromResult(View("NotFound"));
    }
}

