using System;
using System.Linq;
using AutoValidator.Interfaces;
using AutoValidator.Models;

namespace AutoValidator.Impl
{
    public class MapperConfiguration : IConfigurationProvider
    {
        private readonly MapperConfigurationExpression _configurationExpression;

        public MapperConfiguration(Action<IMapperConfigurationExpression> configure)
            : this(Build(configure))
        {
        }

        public MapperConfiguration(MapperConfigurationExpression configurationExpression)
        {
            _configurationExpression = configurationExpression;
        }

        public void AssertExpressionsAreValid()
        {
            if (_configurationExpression == null)
            {
                throw new ConfigurationExpressionException("No configuration has been setup");
            }

            var validationResults = _configurationExpression.GetConfigurationExpressionValidation();
            if (validationResults.Any(v => !v.Success))
            {
                throw new ConfigurationExpressionException(validationResults.Where(v => !v.Success).ToList());
            }
        }

        public IValidatorFactory CreateFactory()
        {
            if(_configurationExpression == null) throw new ArgumentNullException("", "Configuration has not been configured");

            return new ValidatorFactory(_configurationExpression);
        }

        public Func<IValidatorFactory> CreateFactoryFunc()
        {
            return CreateFactory;
        }

        private static MapperConfigurationExpression Build(Action<IMapperConfigurationExpression> configureFunc)
        {
            var expr = new MapperConfigurationExpression();
            configureFunc(expr);
            return expr;
        }
    }
}
