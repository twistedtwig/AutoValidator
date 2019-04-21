using System.Collections.Generic;
using AutoValidator.Helpers;

namespace AutoValidator.Models
{
    public class ValidationResult
    {
        public ValidationResult()
        {
            Errors = new Dictionary<string, List<string>>();
        }

        public bool Success { get; set; }
        public Dictionary<string, List<string>> Errors { get; set; }

        public static ValidationResult SuccessResult => new ValidationResult {Success = true};

        public void AddError(string propName, string errorMessage)
        {
            Errors.AddItemToList(propName, errorMessage);
        }
    }
}
