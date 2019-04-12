using AutoValidator.Models;

namespace AutoValidator.Interfaces
{
    public interface IValidator
    {        
        ValidationResult Validate();

        IValidator IsEmailAddress(string email, string propName = "email", string message = null);
        IValidator NotNullOrEmpty(string text, string propName, string message = null);
        IValidator MinLength(string text, int minLength, string propName, string message = null);
        IValidator MaxLength(string text, int maxLength, string propName, string message = null);


        //https://github.com/gnpretorius/simple-validator
    }

    public interface IValidatorExpression
    {
        void IsEmailAddress(string message = null);
        void NotNullOrEmpty(string message = null);
        void MinLength(int minLength, string message = null);
        void MaxLength(int maxLength, string message = null);
    }
}