using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using AutoValidator.Constants;
using AutoValidator.Interfaces;

namespace AutoValidator.Impl
{
    public class ValidationExpressionErrorMessageFactory<T, TMember> : ValidatorErrorMessageFactoryBase<T, TMember>, IValidationExpressionErrorMessageFactory<T, TMember>
    {
        private Expression<Func<TMember, IValidatorExpression, bool>> _memberExpression;
        private Expression<Func<TMember, T, IValidatorExpression, bool>> _objectMemberExpression;

        public void SetupExpression(Expression<Func<TMember, IValidatorExpression, bool>> exp)
        {
            if (_objectMemberExpression != null)
            {
                throw new ArgumentException("object member expression is already set, cannot set both expressions");
            }

            _memberExpression = exp ?? throw new ArgumentNullException("exp", "member expression must be provided");
        }

        public void SetupExpression(Expression<Func<TMember, T, IValidatorExpression, bool>> exp)
        {
            if (_memberExpression != null)
            {
                throw new ArgumentException("member expression is already set, cannot set both expressions");
            }

            _objectMemberExpression = exp ?? throw new ArgumentNullException("exp", "object member expression must be provided");
        }


        public Tuple<string, List<object>> Invoke(T obj)
        {
            if (string.IsNullOrWhiteSpace(PropName))
            {
                throw new ArgumentNullException("Propname", "Propname must be set before invoke is executed");
            }

            if (_memberExpression == null && _objectMemberExpression == null)
            {
                throw new ArgumentNullException("exp", "one of the member expressions must be set before invoke is executed");
            }

            MethodCallExpression methodCall = null;
            if (_memberExpression != null)
            {
                methodCall = _memberExpression.Body as MethodCallExpression;
            }

            else if (_objectMemberExpression != null)
            {
                methodCall = _objectMemberExpression.Body as MethodCallExpression;
            }

            if (methodCall == null)
            {
                throw new ArgumentOutOfRangeException("exp", "No valid expression found");
            }

            return ExamineMethods(obj, methodCall, PropName);
        }

        private Tuple<string, List<object>> ExamineMethods(T obj, MethodCallExpression methodCall, string propName)
        {
            var methodSignature = methodCall.Method.ToString();

            string format = string.Empty;
            string messageOverride = string.Empty;
            var variables = new List<object>();

            switch (methodSignature)
            {
                case "Boolean IsEmailAddress(System.String, System.String)":
                    messageOverride = GetArgumentMessageOverride(obj, methodCall, 1);
                    format = !string.IsNullOrWhiteSpace(messageOverride) ? messageOverride : ValidationMessageConstStrings.InvalidEmail;
                    break;

                case "Boolean NotNullOrEmpty(System.String, System.String)":
                    messageOverride = GetArgumentMessageOverride(obj, methodCall, 1);
                    format = !string.IsNullOrWhiteSpace(messageOverride) ? messageOverride : ValidationMessageConstStrings.StringNotNullOrEmpty;
                    break;

                case "Boolean MaxLength(System.String, Int32, System.String)":
                    messageOverride = GetArgumentMessageOverride(obj, methodCall, 2);
                    format = !string.IsNullOrWhiteSpace(messageOverride) ? messageOverride : ValidationMessageConstStrings.StringMaxLength;

                    variables.AddRange(GetArgumentValue(obj, methodCall, methodCall.Arguments[1]));
                    break;

                case "Boolean MinLength(System.String, Int32, System.String)":
                    messageOverride = GetArgumentMessageOverride(obj, methodCall, 2);
                    format = !string.IsNullOrWhiteSpace(messageOverride) ? messageOverride : ValidationMessageConstStrings.StringMinLength;

                    variables.AddRange(GetArgumentValue(obj, methodCall, methodCall.Arguments[1]));
                    break;

                case "Boolean MinValue(Int32, Int32, System.String)":
                    messageOverride = GetArgumentMessageOverride(obj, methodCall, 2);
                    format = !string.IsNullOrWhiteSpace(messageOverride) ? messageOverride : ValidationMessageConstStrings.IntMinValue;

                    variables.AddRange(GetArgumentValue(obj, methodCall, methodCall.Arguments[1]));
                    break;

                case "Boolean Ignore()":
                    format = ValidationMessageConstStrings.Ignore;
                    break;

                default:
                    throw new ArgumentOutOfRangeException("unknown IValidatorExpression expression method");
            }

            variables.Add(propName);
            
            return new Tuple<string, List<object>>(format, variables);
        }        
    }
}
