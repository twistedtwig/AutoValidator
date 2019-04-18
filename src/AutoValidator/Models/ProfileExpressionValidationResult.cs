using System.Collections.Generic;

namespace AutoValidator.Models
{
    public class ProfileExpressionValidationResult
    {
        public bool Success { get; set; }
        public List<ConfigurationClassExpressionValidationResult> ExpressionErrors { get; set; }

        public ProfileExpressionValidationResult()
        {
            Success = false;
            ExpressionErrors = new List<ConfigurationClassExpressionValidationResult>();
        }
    }
}
