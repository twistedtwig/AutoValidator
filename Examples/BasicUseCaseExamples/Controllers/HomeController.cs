using AutoValidator.Impl;
using AutoValidator.Interfaces;
using Microsoft.AspNetCore.Mvc;
using BasicUseCaseExamples.Models;

namespace BasicUseCaseExamples.Controllers
{
    public class UserModelProfile : ClassValidationProfile
    {
        public UserModelProfile()
        {
            CreateMap<UserModel>()
                .ForMember(x => x.Name, (name, exp) => exp.MinLength(name, 4, null))
                .ForMember(x => x.Name, (name, exp) => exp.MaxLength(name, 99, null))
                .ForMember(x => x.Age, (age, exp) => exp.MinValue(age, 18, null));
        }
    }
   

    public class HomeController : Controller
    {
        private readonly IValidatorFactory _factory;

        public HomeController()
        {
            // for simplicity this is here, but in a real world you would initialize it at the root of your application (see DependencyInjectionExample)
            var validation = new AutoValidation(cfg => cfg.AddProfile<UserModelProfile>());
            _factory = validation.CreateFactory();
        }
        
        [HttpPost]
        public IActionResult SimpleValidator(string name, int age)
        {
            var validator = new Validator();

            var validationResult = validator
                .MinLength(name, 4, "Name")
                .MaxLength(name, 99, "Name")
                .MinValue(age, 18, "Age")
                .Validate();

            if (!validationResult.Success)
            {
                return BadRequest(validationResult.Errors);
            }

            // this is where you would call your business logic

            return Ok();
        }

        [HttpPost]
        public IActionResult SimpleValidatorUsingCustomExpression(string name, int age)
        {
            var validator = new Validator();

            var validationResult = validator
                .Custom(name, x => x.Length >= 4, "name", "{0} should be at least 4 characters long")
                .Custom(name, x => x.Length <= 99, "name", "{0} should be less than or equal to 99 characters long")
                .Custom(age, x => x >= 18, "age", "{0} must be at least 18 years old")                
                .Validate();

            if (!validationResult.Success)
            {
                return BadRequest(validationResult.Errors);
            }

            // this is where you would call your business logic

            return Ok();
        }

        [HttpPost]
        public IActionResult SimpleValidatorUsingProfile([FromBody] UserModel request)
        {
            var validator = _factory.Create<UserModel>();

            var validationResult = validator.Validate(request);

            if (!validationResult.Success)
            {
                return BadRequest(validationResult.Errors);
            }

            // this is where you would call your business logic

            return Ok();
        }
    }
}
