using System;
using System.Collections.Generic;

namespace AutoValidator.Models
{
    public class ConfigurationExpressionException : Exception
    {
        private readonly List<ProfileExpressionValidationResult> _errors;

        public IEnumerable<ProfileExpressionValidationResult> Errors => _errors;

        public ConfigurationExpressionException()
        {
            
        }

        public ConfigurationExpressionException(string message) : base(message)
        {
            
        }

        public ConfigurationExpressionException(List<ProfileExpressionValidationResult> errors)
        {
            _errors = errors;
        }
    }
}
