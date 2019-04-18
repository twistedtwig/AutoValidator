using System.Collections.Generic;
using AutoValidator.Models;

namespace AutoValidator.Interfaces
{
    public interface IClassValidationProfile
    {
        IEnumerable<ITypeMapConfiguration> ValidationExpressions { get; }
        IMappingExpression<T> CreateMap<T>();

        ProfileExpressionValidationResult ValidateExpression();
    }
}
