using System;
using System.Collections.Generic;
using AutoValidator.Interfaces;
using AutoValidator.Models;

namespace AutoValidator.Impl
{
    public class Validator : IValidator
    {
        private readonly ValidationExpressionErrorMessageFactory _errorMessageFactory;
        private readonly ClassValidatorExpression _expressionValidator;
        private readonly Dictionary<string, string> _errors;

        public Validator()
        {
            _errorMessageFactory = new ValidationExpressionErrorMessageFactory();
            _expressionValidator = new ClassValidatorExpression();
            _errors = new Dictionary<string, string>();
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

        //TODO will use IValidationExpressionErrorMessageFactory to generate final error message

        public IValidator IsEmailAddress(string email, string propName = "email", string message = null)
        {
            var valid = _expressionValidator.IsEmailAddress(email, message);

            if (!valid)
            {
                var result = _errorMessageFactory.Get<string>((val, exp) => exp.IsEmailAddress(email, message), propName);

                LogError(propName, result);
            }

            return this;
        }

        public IValidator NotNullOrEmpty(string text, string propName, string message = null)
        {
            if (!_expressionValidator.NotNullOrEmpty(text))
            {
                var result = _errorMessageFactory.Get<string>((val, exp) => exp.NotNullOrEmpty(text, message), propName);

                LogError(propName, result);
            }

            return this;
        }

        public IValidator MinLength(string text, int minLength, string propName, string message = null)
        {
            if (!_expressionValidator.MinLength(text, minLength, message))
            {
                var result = _errorMessageFactory.Get<string>((val, exp) => exp.MinLength(text, minLength, message), propName);

                LogError(propName, result);
            }

            return this;
        }

        public IValidator MaxLength(string text, int maxLength, string propName, string message = null)
        {
            if (!_expressionValidator.MaxLength(text, maxLength, message))
            {
                var result = _errorMessageFactory.Get<string>((val, exp) => exp.MaxLength(text, maxLength, message), propName);

                LogError(propName, result);
            }

            return this;
        }

        public IValidator MinValue(int value, int min, string propName, string message = null)
        {
            if (!_expressionValidator.MinValue(value, min, message))
            {                
                var result = _errorMessageFactory.Get<int>((val, exp) => exp.MinValue(val, min, message), propName);

                LogError(propName, result);
            }

            return this;
        }

        private void LogError(string propName, Tuple<string, List<object>> messageValue)
        {
            EnsureUniquePropName(propName);
            _errors.Add(propName, string.Format(messageValue.Item1, messageValue.Item2.ToArray()));
        }

        private void EnsureUniquePropName(string name)
        {
            if (_errors.ContainsKey(name))
            {
                throw new ArgumentException($"key '{name}' has already been used and errored");
            }
        }
    }
}
