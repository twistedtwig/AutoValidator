using System.Collections.Generic;
using System.Linq;
using AutoValidator.Interfaces;
using AutoValidator.Models;

namespace AutoValidator.Impl
{
    public abstract class ClassValidationProfile : IClassValidationProfile
    {
        private readonly List<ITypeMapConfiguration> _mappings;

        protected ClassValidationProfile()
        {
            _mappings = new List<ITypeMapConfiguration>();
        }

        public IEnumerable<ITypeMapConfiguration> ValidationExpressions => _mappings.AsEnumerable();

        public IMappingExpression<T> CreateMap<T>()
        {
            var expression = new MappingExpression<T>();
            _mappings.Add(expression);

            return expression;
        }

        public ProfileExpressionValidationResult ValidateExpression()
        {
            var expressionMappings = new List<ConfigurationClassExpressionValidationResult>();
            foreach (var mapping in _mappings)
            {
                expressionMappings.Add(mapping.ValidateExpression());
            }

            var result = new ProfileExpressionValidationResult
            {
                Success = expressionMappings.All(x => x.Success),
                ExpressionErrors = expressionMappings
            };

            return result;
        }
    }
}
