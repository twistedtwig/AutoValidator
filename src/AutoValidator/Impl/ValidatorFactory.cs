using System;
using System.Linq;
using AutoValidator.Interfaces;

namespace AutoValidator.Impl
{
    public class ValidatorFactory : IValidatorFactory
    {
        private readonly MapperConfigurationExpression _configurationExpression;

        public ValidatorFactory(MapperConfigurationExpression configExpression)
        {
            _configurationExpression = configExpression;
        }

        public IValidator Create()
        {
            return new Validator();
        }

        public IClassValidator<T> Create<T>() where T : class
        {
            var expression = _configurationExpression.Profiles.SelectMany(p => p.MappingExpressions).OfType<IMappingExpression<T>>().FirstOrDefault(exp => exp.SourceType == typeof(T));
            if(expression == null) throw new ArgumentNullException("", "unmapped model requested");

            var validator = new ClassValidator<T>(expression);

            return validator;
        }
    }
}