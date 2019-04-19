using System.Collections.Generic;
using AutoValidator.Models;

namespace AutoValidator.Interfaces
{
    public interface IClassValidationProfile
    {
        IEnumerable<ITypeMapConfiguration> MappingExpressions { get; }
        IMappingExpression<T> CreateMap<T>();

        ProfileExpressionValidationResult ValidateExpression();
    }
}
