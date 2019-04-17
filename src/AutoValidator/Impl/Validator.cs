using System;
using System.Collections.Generic;
using AutoValidator.Constants;
using AutoValidator.Interfaces;
using AutoValidator.Models;

namespace AutoValidator.Impl
{
    public class Validator : IValidator
    {
        private readonly ClassValidatorExpression _expressionValidator;
        private readonly Dictionary<string, string> _errors;

        public Validator()
        {
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
                LogError(propName, message ?? ValidationMessageConstStrings.InvalidEmail);

            }

            return this;
        }

        public IValidator NotNullOrEmpty(string text, string propName, string message = null)
        {
            if (!_expressionValidator.NotNullOrEmpty(text))
            {
                LogError(propName, string.Format(message ?? ValidationMessageConstStrings.StringNotNullOrEmpty, propName));
            }

            return this;
        }

        public IValidator MinLength(string text, int minLength, string propName, string message = null)
        {
            if (!_expressionValidator.MinLength(text, minLength, message))
            {
                LogError(propName, string.Format(message ?? ValidationMessageConstStrings.StringMinLength, minLength, propName));
            }

            return this;
        }

        public IValidator MaxLength(string text, int maxLength, string propName, string message = null)
        {
            if (!_expressionValidator.MaxLength(text, maxLength, message))
            {
                LogError(propName, string.Format(message ?? ValidationMessageConstStrings.StringMaxLength, maxLength, propName));
            }

            return this;
        }

        public IValidator MinValue(int value, int min, string propName, string message = null)
        {
            if (!_expressionValidator.MinValue(value, min, message))
            {
                LogError(propName, string.Format(message ?? ValidationMessageConstStrings.IntMinValue, min, propName));
            }

            return this;
        }

        private void LogError(string propName, string message)
        {
            EnsureUniquePropName(propName);
            _errors.Add(propName, message);
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
