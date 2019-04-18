using AutoValidator.Models;

namespace AutoValidator.Interfaces
{
    public interface IClassValidationProfile
    {
        IMappingExpression<T> CreateMap<T>();

        ProfileExpressionValidationResult ValidateExpression();
    }
}
