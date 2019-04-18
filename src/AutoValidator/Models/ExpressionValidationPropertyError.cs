
namespace AutoValidator.Models
{
    public class ExpressionValidationPropertyError
    {
        public ExpressionValidationPropertyError(string propName, string error)
        {
            PropertyName = propName;
            Error = error;
        }

        public string PropertyName { get; set; }
        public string Error { get; set; }
    }
}
