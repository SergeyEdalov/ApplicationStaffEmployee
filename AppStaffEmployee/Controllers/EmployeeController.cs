using AppStaffEmployee.Models.Dto;
using AppStaffEmployee.Services.Interfaces;
using AppStaffEmployee.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationStaffEmployee.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService<EmployeeDto, Guid> _employeeService;
        private readonly ILogger<EmployeeController> _logger;
        private readonly IMapper _mapper;

        public EmployeeController(IEmployeeService<EmployeeDto, Guid> employeeService, ILogger<EmployeeController> logger, IMapper mapper)
        {
            _employeeService = employeeService;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var employeesTable = await _employeeService.GetAllEmployeesAsync();
            var employeeView = employeesTable.Select(_mapper.Map<EmployeeViewModel>);
            return View(employeeView);
        }

        public async Task<IActionResult> Details (Guid id)
        {
            var employee = await _employeeService.GetEmpoloyeeByIDAsync(id);
            if (employee is null)  return NotFound();

            var employeeView = _mapper.Map<EmployeeViewModel>(employee);
            return View(employeeView);
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var employee = await _employeeService.GetEmpoloyeeByIDAsync(id);
            if (employee is null) return NotFound();

            var employeeView = _mapper.Map<EmployeeViewModel>(employee);
            return View(employeeView);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EmployeeViewModel model)
        {
            var employeeDto = _mapper.Map<EmployeeDto>(model);
            var success = await _employeeService.EditEmployeeAsync(employeeDto);
            
            if(!success) return NotFound();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            return View();
        }

        //public IActionResult Privacy()
        //{
        //    return View();
        //}

        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}

        //public async Task<IActionResult> CreateEmployeeAsync(RequestCRUD request)
        //{
        //    var response = new ResponseCRUD(await _employeeService.CreateEmployeeAsync(request.EmployeeDto), null);

        //    return View(response);
        //}

        //public async Task<IActionResult> UpdateEmployeeAsync(RequestCRUD request)
        //{
        //    if (request.EmployeeId != null)
        //    {
        //        var response = new ResponseCRUD(await _employeeService.UpdateEmployeeAsync((Guid)request.EmployeeId, request.EmployeeDto), null);
        //        return View(response);
        //    }
        //    return View("Mistake");
        //}

        //public async Task<IActionResult> DeleteEmployeeAsync(RequestCRUD request)
        //{
        //    if (request.EmployeeId != null)
        //    {
        //        var response = new ResponseCRUD(await _employeeService.DeleteEmployeeAsync((Guid)request.EmployeeId), null);
        //        return View(response);
        //    }
        //    return View("Mistake");
        //}

        //public async Task<IActionResult> GetEmployeeAsync(RequestCRUD request)
        //{

        //    var response = new ResponseCRUD(true, await _employeeService.GetAllEmployeesAsync());
        //    return View(response);
        //}
    }
}
