using System.Collections.Generic;
using AutoValidator.Helpers;

namespace AutoValidator.Models
{
    public class ValidationResult
    {
        private readonly bool _useCamelCase = false;

        public ValidationResult()
        {
            Errors = new Dictionary<string, List<string>>();
        }

        public ValidationResult(ValidatorSettings settings, bool success = true) : this()
        {
            _useCamelCase = settings.UseCamelCase;
            Success = success;
        }

        public bool Success { get; set; }
        public Dictionary<string, List<string>> Errors { get; set; }

        public static ValidationResult SuccessResult => new ValidationResult {Success = true};

        public void AddError(string propName, string errorMessage)
        {
            var name = _useCamelCase ? propName.ToCamelCase() : propName;
            Errors.AddItemToList(name, errorMessage);
        }
    }
}
