using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using AutoValidator.Helpers;
using AutoValidator.Interfaces;
using AutoValidator.Models;

namespace AutoValidator.Impl
{
    public class Validator : IValidator
    {
        private readonly ClassValidatorExpression _expressionValidator;
        private readonly ValidatorSettings _settings;
        private readonly Dictionary<string, List<string>> _errors;

        public Validator(ValidatorSettings settings = null)
        {
            _expressionValidator = new ClassValidatorExpression();
            _errors = new Dictionary<string, List<string>>();

            _settings = settings ?? new ValidatorSettings();
        }

        public ValidationResult Validate()
        {
            var result = new ValidationResult
            {
                Success = _errors.Keys.Count == 0,
                Errors = _errors
            };

            return result;
        }
        
        public IValidator IsEmailAddress(string email, string propName = "email", string message = null)
        {
            var valid = _expressionValidator.IsEmailAddress(email, message);

            if (!valid)
            {
                var result = getErrorMessage((val, exp) => exp.IsEmailAddress(email, message), email, propName);

                LogError(propName, result);
            }

            return this;
        }

        public IValidator NotNullOrEmpty(string text, string propName, string message = null)
        {
            if (!_expressionValidator.NotNullOrEmpty(text))
            {
                var result = getErrorMessage((val, exp) => exp.NotNullOrEmpty(text, message), text, propName);

                LogError(propName, result);
            }

            return this;
        }

        public IValidator MinLength(string text, int minLength, string propName, string message = null)
        {
            if (!_expressionValidator.MinLength(text, minLength, message))
            {
                var result = getErrorMessage((val, exp) => exp.MinLength(text, minLength, message), text, propName);

                LogError(propName, result);
            }

            return this;
        }

        public IValidator MaxLength(string text, int maxLength, string propName, string message = null)
        {
            if (!_expressionValidator.MaxLength(text, maxLength, message))
            {
                var result = getErrorMessage((val, exp) => exp.MaxLength(text, maxLength, message), text, propName);

                LogError(propName, result);
            }

            return this;
        }

        public IValidator MinValue(int value, int min, string propName, string message = null)
        {
            if (!_expressionValidator.MinValue(value, min, message))
            {                
                var result = getErrorMessage((val, exp) => exp.MinValue(val, min, message), value, propName);

                LogError(propName, result);
            }

            return this;
        }

        public IValidator IsNotNull(object obj, string propName, string message = null)
        {
            if (!_expressionValidator.IsNotNull(obj))
            {
                var result = getErrorMessage((val, exp) => exp.IsNotNull(val, message), obj, propName);

                LogError(propName, result);
            }

            return this;
        }

        public IValidator Custom<TMember>(TMember value, Func<TMember, bool> memberValidationFunc, string propName, string errorMessage)
        {
            if (!memberValidationFunc.Invoke(value))
            {
                LogError(propName, new Tuple<string, List<object>>(errorMessage, new List<object> { propName , value }));
            }

            return this;
        }

        public IValidator Custom<T, TMember>(T value, Expression<Func<T, TMember>> memberSelectorExpression, Func<TMember, bool> memberValidationFunc, string errorMessage) where T : class
        {
            TMember member = memberSelectorExpression.Compile().Invoke(value);

            if (!memberValidationFunc.Invoke(member))
            {
                var memberInfo = ReflectionHelper.FindProperty(memberSelectorExpression);
                var propName = memberInfo.Name;

                LogError(propName, new Tuple<string, List<object>>(errorMessage, new List<object> { propName, member }));
            }

            return this;
        }

        private Tuple<string, List<object>> getErrorMessage<T>(Expression<Func<T, IValidatorExpression, bool>> expression, T value, string propName)
        {
            var errorMessageFactory = new ValidationExpressionErrorMessageFactory<T, T>();
            errorMessageFactory.SetPropName(propName);
            errorMessageFactory.SetupExpression(expression);
            var messageDetails = errorMessageFactory.Invoke(value);

            // add the value into the values for string format so that it can be used in custom error messages.
            messageDetails.Item2.Add(value);

            return messageDetails;
        }

        private void LogError(string propName, Tuple<string, List<object>> messageValue)
        {
            var name = _settings.UseCamelCase ? propName.ToCamelCase() : propName;
            var errorMessage = string.Format(messageValue.Item1, messageValue.Item2.ToArray());
            _errors.AddItemToList(name, errorMessage);
        }
    }
}
