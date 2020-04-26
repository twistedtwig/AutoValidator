using System;
using System.Linq;
using AutoValidator.Interfaces;
using AutoValidator.Models;

namespace AutoValidator.Impl
{
    public class AutoValidation : IConfigurationProvider
    {
        private readonly ValidatorConfigurationExpression _configurationExpression;

        public AutoValidation(Action<IValidatorConfigurationExpression> configure)
            : this(Build(configure))
        {
        }

        public AutoValidation(ValidatorConfigurationExpression configurationExpression)
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

        public bool HasMap(Type t)
        {
            foreach (var profile in _configurationExpression.Profiles)
            {
                foreach (var mappingExpression in profile.MappingExpressions)
                {
                    if (mappingExpression.SourceType == t)
                    {
                        return true;
                    }
                }
            }

            return false;
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

        private static ValidatorConfigurationExpression Build(Action<IValidatorConfigurationExpression> configureFunc)
        {
            var expr = new ValidatorConfigurationExpression();
            configureFunc(expr);
            return expr;
        }
    }
}
