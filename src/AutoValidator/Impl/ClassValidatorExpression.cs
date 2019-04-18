using System;
using System.Text.RegularExpressions;
using AutoValidator.Interfaces;

namespace AutoValidator.Impl
{
    public class ClassValidatorExpression : IValidatorExpression
    {
        public bool Ignore()
        {
            // doesn't need to do anything.  it is just here to ensure we can tell system to ignore a property.
            return true;
        }

        public bool IsEmailAddress(string email, string message = null)
        {
            bool valid = false;
            if (!string.IsNullOrWhiteSpace(email))
            {
                string exp = @"^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-||_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+([a-z]+|\d|-|\.{0,1}|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])?([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))$";

                valid = new Regex(exp, RegexOptions.IgnoreCase).IsMatch(email);
            }

            return valid;
        }

        public bool NotNullOrEmpty(string text, string message = null)
        {
            return !string.IsNullOrWhiteSpace(text);
        }

        public bool MinLength(string text, int minLength, string message = null)
        {
            return text != null && text.Length >= minLength;
        }

        public bool MaxLength(string text, int maxLength, string message = null)
        {
            return text != null && text.Length <= maxLength;
        }

        public bool MinValue(int value, int min, string message = null)
        {
            return value >= min;
        }
    }
}
