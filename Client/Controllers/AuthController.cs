using API.DTOs.Accounts;
using API.DTOs.Employees;
using API.Models;
using Client.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Client.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAccountRepository _accountRepository;

        public AuthController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public IActionResult Login()
        {
            return View();
        }


        [HttpGet("Logout/")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Auth");
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto login)
        {




            var result = await _accountRepository.Login(login);

            if (result.Status == "OK")
            {

                HttpContext.Session.SetString("JWToken", result.Data.Token);
                return RedirectToAction("Index", "Latihan");
            }
            return View();
        }



        /*       private readonly IAccountRepository repository;

               public AccountController(IAccountRepository repository)
               {
                   this.repository = repository;
               }

               public IActionResult Login()
               {
                   return View();
               }*/

        /* [HttpPost]
         public IActionResult Login(LoginDto login)
         {
             var result = _accountRepository.Login(login).Result;

             if (result.Status == "OK")
             {
                 return RedirectToAction("Index", "Home");
             }
             return View();
         }*/

        /*
                [HttpPost]
                public async Task<IActionResult> Login(LoginDto loginDto)
                {
                    var result = await repository.Post(loginDto); ;
                    Console.WriteLine(result.Data);
                    if (result.Code == 200)
                    {
                        return RedirectToAction("Index", "Employee");
                    }
                    ModelState.AddModelError(string.Empty, result.Message);
                    return View();
                }*/
    }
}
