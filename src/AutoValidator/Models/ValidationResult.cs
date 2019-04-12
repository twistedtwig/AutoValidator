using System.Collections.Generic;

namespace AutoValidator.Models
{
    public class ValidationResult
    {
        public ValidationResult()
        {
            Errors = new Dictionary<string, string>();
        }

        public bool Success { get; set; }
        public Dictionary<string, string> Errors { get; set; }

        public static ValidationResult SuccessResult => new ValidationResult {Success = true};

        public static ValidationResult NullObjectResult => new ValidationResult
        {
            Success = false,
            Errors = new Dictionary<string, string>()
            {
                { "Object", "Null item passed to validator" }
            }
        };
    }
}
