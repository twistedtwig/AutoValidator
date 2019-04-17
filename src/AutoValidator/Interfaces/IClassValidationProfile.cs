using System.Collections.Generic;

namespace AutoValidator.Interfaces
{
    public interface IClassValidationProfile
    {
        IEnumerable<ITypeMapConfiguration> ValidationExpressions { get; }
        IMappingExpression<T> CreateMap<T>();
    }
}
