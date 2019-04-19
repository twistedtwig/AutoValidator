using System;
using System.Collections.Generic;

namespace AutoValidator.Models
{
    public class ProfileExpressionValidationResult
    {
        public Type ProfileType { get; }
        public bool Success { get; set; }
        public List<ClassExpressionValidationResult> ExpressionResults { get; set; }

        public ProfileExpressionValidationResult(Type profile)
        {
            ProfileType = profile;
            Success = false;
            ExpressionResults = new List<ClassExpressionValidationResult>();
        }
    }
}
