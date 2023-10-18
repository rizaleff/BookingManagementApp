using API.DTOs.Employees;
using API.Models;
using Client.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Client.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository repository;

        public EmployeeController(IEmployeeRepository repository)
        {
            this.repository = repository;
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EmployeeDto employee)
        {
            var result = await repository.Put(employee.Guid, employee); 
            if (result.Code == 200)
            {
                return RedirectToAction(nameof(Index));
            }
            ModelState.AddModelError(string.Empty, result.Message);
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var result = await repository.Get();
            var listEmployee = new List<EmployeeDto>();
            if (result != null)
            {
                listEmployee = result.Data.Select(x => (EmployeeDto)x).ToList();
            }

            return View(listEmployee);
        }


        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            Console.WriteLine(id);
            var result = await repository.Get(id);
            var selectedEmployee = new EmployeeDto();
            if (result.Data != null)
            {
                selectedEmployee = (EmployeeDto)result.Data;
            }
            return View(selectedEmployee);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var result = await repository.Get(id);
            var selectedEmployee = new EmployeeDto();

            if (result.Data != null)
            {
                selectedEmployee = (EmployeeDto)result.Data;
            }
            return View(selectedEmployee);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await repository.Get(id);
            var selectedEmployee = new EmployeeDto();

            if (result.Data != null)
            {
                selectedEmployee = (EmployeeDto)result.Data;
            }
            return View(selectedEmployee);
        }

        [HttpPost]
        public async Task<IActionResult> ToDelete([FromForm] Guid id)
        {
            var result = await repository.Delete(id);;
            if (result.Code == 200)
            {
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        public async Task<IActionResult> Create(CreateEmployeeDto employee)
        {
            var result = await repository.Post(employee);;
            if (result.Code == 200)
            {
                return RedirectToAction(nameof(Index));
            }
            ModelState.AddModelError(string.Empty, result.Message);
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }
    }
}
