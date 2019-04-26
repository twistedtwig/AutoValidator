using AutoValidator.Impl;
using AutoValidator.Interfaces;
using Microsoft.AspNetCore.Mvc;
using BasicUseCaseExamples.Models;

namespace BasicUseCaseExamples.Controllers
{
    public class UserModel2Profile : ClassValidationProfile
    {
        public UserModel2Profile()
        {
            CreateMap<UserModel>()
                .ForMember(x => x.Name, (name, exp) => exp.MinLength(name, 4, "please make sure your name is at least {1} characters long"))
                .ForMember(x => x.Name, (name, exp) => exp.MaxLength(name, 99, "your name is too long"))
                .ForMember(x => x.Age, (age, exp) => exp.MinValue(age, 18, "you must be at least {1} years old"));
        }
    }
   

    public class ErrorMessageExamplesController : Controller
    {
        private readonly IValidatorFactory _factory;

        public ErrorMessageExamplesController()
        {
            // for simplicity this is here, but in a real world you would initialize it at the root of your application (see DependencyInjectionExample)
            var validation = new AutoValidation(cfg => cfg.AddProfile<UserModel2Profile>());
            _factory = validation.CreateFactory();
        }
        
        [HttpPost]
        public IActionResult SimpleValidator(string name, int age)
        {
            var validator = new Validator();

            var validationResult = validator
                .MinLength(name, 4, "Name", "{0} should be >= {1}")
                .MaxLength(name, 99, "Name", "{0} can not be bigger than {1}")
                .MinValue(age, 18, "Age", "You must be older than 17")
                .Validate();

            if (!validationResult.Success)
            {
                // for the error messages
                // first param would be the propname, second in these examples would be the value 4 for minLength or 99 for MaxLength

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
                // for the error messages
                // customs can only use propname as a string format value.
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
