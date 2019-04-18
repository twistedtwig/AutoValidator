using System;
using System.Collections.Generic;

namespace AutoValidator.Models
{
    public class ConfigurationClassExpressionValidationResult
    {
        public Type SourceClass { get; set; }
        public bool Success { get; set; }
        public List<string> PropertiesThatHaveErrors { get; set; }
        public List<string> Errors { get; set; }

        public ConfigurationClassExpressionValidationResult()
        {
            Success = false;
            PropertiesThatHaveErrors = new List<string>();
            Errors = new List<string>();
        }
    }
}
