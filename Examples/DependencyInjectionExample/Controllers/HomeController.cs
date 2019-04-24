using System.Diagnostics;
using AutoValidator.Interfaces;
using Microsoft.AspNetCore.Mvc;
using DependencyInjectionExample.Models;

namespace DependencyInjectionExample.Controllers
{
    public class HomeController : Controller
    {
        private readonly IValidatorFactory _validatorFactory;

        public HomeController(IValidatorFactory validatorFactory)
        {
            _validatorFactory = validatorFactory;
        }

        public IActionResult Index()
        {
            var validator = _validatorFactory.Create<Model1>();

            var model = new Model1
            {
                Age = 21,
                Name = "Jon Hawkins",
                Category = "cat1"
            };

            var result = validator.Validate(model);

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
