using System;
using System.Collections.Generic;

namespace AutoValidator.Models
{
    public class ClassExpressionValidationResult
    {
        public Type SourceClass { get; set; }
        public bool Success { get; set; }
        public List<ExpressionValidationPropertyError> Errors { get; set; }

        public ClassExpressionValidationResult(Type source)
        {
            SourceClass = source;
            Success = false;
            Errors = new List<ExpressionValidationPropertyError>();
        }
    }
}
