using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using AutoValidator.Constants;
using AutoValidator.Interfaces;
using AutoValidator.Models;

namespace AutoValidator.Impl
{
    public class Validator : IValidator
    {
        private readonly Dictionary<string, string> _errors;

        public Validator()
        {
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

        public IValidator IsEmailAddress(string email, string propName = "email", string message = null)
        {
            bool valid = false;
            if (!string.IsNullOrWhiteSpace(email))
            {
                string exp = @"^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-||_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+([a-z]+|\d|-|\.{0,1}|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])?([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))$";

                valid = new Regex(exp, RegexOptions.IgnoreCase).IsMatch(email);
            }

            if (!valid)
            {
                LogError(propName, message ?? ValidationMessageConstStrings.InvalidEmail);

            }

            return this;
        }

        public IValidator NotNullOrEmpty(string text, string propName, string message = null)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                LogError(propName, string.Format(message ?? ValidationMessageConstStrings.NotNullOrEmpty, propName));
            }

            return this;
        }

        public IValidator MinLength(string text, int minLength, string propName, string message = null)
        {
            if (text != null && text.Length < minLength)
            {
                LogError(propName, string.Format(message ?? ValidationMessageConstStrings.MinLength, minLength, propName));
            }

            return this;
        }

        public IValidator MaxLength(string text, int maxLength, string propName, string message = null)
        {
            if (text != null && text.Length > maxLength)
            {
                LogError(propName, string.Format(message ?? ValidationMessageConstStrings.MaxLength, maxLength, propName));
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
