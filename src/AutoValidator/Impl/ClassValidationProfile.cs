using System.Collections.Generic;
using AutoValidator.Interfaces;

namespace AutoValidator.Impl
{
    public abstract class ClassValidationProfile : IClassValidationProfile
    {
        private readonly List<ITypeMapConfiguration> _mappings;

        protected ClassValidationProfile()
        {
            _mappings = new List<ITypeMapConfiguration>();
        }

        public IMappingExpression<T> CreateMap<T>()
        {
            var expression = new MappingExpression<T>();
            _mappings.Add(expression);

            return expression;
        }
    }
}
