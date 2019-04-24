using System;
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

        /// <summary>
        /// Custom validation expression
        /// </summary>
        /// <typeparam name="TMember">The type to be validated</typeparam>
        /// <param name="value">Value to be validated</param>
        /// <param name="memberValidationFunc">Function to validate the value</param>
        /// <param name="propName">name of the property to be used for error message(s)</param>
        /// <param name="errorMessage">Error message string format {0} will be the property name</param>
        /// <returns></returns>
        IValidator Custom<TMember>(TMember value, Func<TMember, bool> memberValidationFunc, string propName, string errorMessage);
    }

    public interface IValidatorExpression
    {
        bool Ignore();

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