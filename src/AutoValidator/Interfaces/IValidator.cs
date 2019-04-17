using AutoValidator.Models;

namespace AutoValidator.Interfaces
{
    public interface IValidator
    {        
        ValidationResult Validate();

        IValidator IsEmailAddress(string email, string propName = "email", string message = null);

        //string expressions
        IValidator NotNullOrEmpty(string text, string propName, string message = null);
        IValidator MinLength(string text, int minLength, string propName, string message = null);
        IValidator MaxLength(string text, int maxLength, string propName, string message = null);


        //int expressions 
        /// <summary>
        /// Is the value at least equal to the min
        /// </summary>
        /// <param name="value"></param>
        /// <param name="min"></param>
        /// <param name="propName"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        IValidator MinValue(int value, int min, string propName, string message = null);

        //https://github.com/gnpretorius/simple-validator
    }

    public interface IValidatorExpression
    {
        bool IsEmailAddress(string email, string message = null);

        //string expressions
        bool NotNullOrEmpty(string text, string message = null);
        bool MinLength(string text, int minLength, string message = null);
        bool MaxLength(string text, int maxLength, string message = null);

        //int expressions
        /// <summary>
        /// Is the value at least equal to the min
        /// </summary>
        /// <param name="value"></param>
        /// <param name="min"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        bool MinValue(int value, int min, string message = null);
    }

}