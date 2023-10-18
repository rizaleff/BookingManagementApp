using API.DTOs.Employees;
using API.Models;
using Client.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Client.Controllers
{
    public class UniversityController : Controller
    {
        private readonly IUniversityRepository repository;

        public UniversityController(IUniversityRepository repository)
        {
            this.repository = repository;
        }
        
        public IActionResult Index()
        {
            return View();
        }
        public async Task<JsonResult> GetAllUniversity()
        {
            var result = await repository.Get();
            return Json(result.Data);
        }

    }
}
