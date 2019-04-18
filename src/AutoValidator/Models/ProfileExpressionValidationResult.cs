using System.Collections.Generic;

namespace AutoValidator.Models
{
    public class ProfileExpressionValidationResult
    {
        public bool Success { get; set; }
        public List<ClassExpressionValidationResult> ExpressionResults { get; set; }

        public ProfileExpressionValidationResult()
        {
            Success = false;
            ExpressionResults = new List<ClassExpressionValidationResult>();
        }
    }
}
