using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using AutoValidator.Interfaces;

namespace AutoValidator.Impl
{
    public class ObjectValidatorErrorMessageFactory<T, TMember> : ValidatorErrorMessageFactoryBase<T, TMember>, IObjectValidatorErrorMessageFactory<T, TMember>
    {
        private string _messageFormatString;
        private Expression<Func<TMember, bool>> _memberExpression;
        private Expression<Func<TMember, T, bool>> _objectMemberExpression;

        public void SetErrorFormat(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentNullException("message", "error message format string must be provided");
            }
            _messageFormatString = message;
        }

        public void SetupExpression(Expression<Func<TMember, bool>> exp)
        {
            if (_objectMemberExpression != null)
            {
                throw new ArgumentException("object member expression is already set, cannot set both expressions");
            }

            _memberExpression = exp ?? throw new ArgumentNullException("exp", "member expression must be provided");
        }

        public void SetupExpression(Expression<Func<TMember, T, bool>> exp)
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

            if (string.IsNullOrWhiteSpace(_messageFormatString))
            {
                throw new ArgumentNullException("ErrorFormat", "error message format must be set before invoke is executed");
            }

            if (_memberExpression == null && _objectMemberExpression == null)
            {
                throw new ArgumentNullException("exp", "one of the member expressions must be set before invoke is executed");
            }

            MethodCallExpression methodCall = null;
            var x1 = _memberExpression as MethodCallExpression;
            var x2 = _memberExpression.Body as MethodCallExpression;

            var x3 = _memberExpression.Parameters;
            var x4 = _memberExpression.Body;
            var x5 = x4.GetType();

            var x6 = _memberExpression.Compile();
            var x7 = x6.GetType();


            if (_memberExpression != null)
            {
                methodCall = _memberExpression.Body as MethodCallExpression;
            }

            else if (_objectMemberExpression != null)
            {
                methodCall = _objectMemberExpression.Body as MethodCallExpression;
            }

            methodCall = _objectMemberExpression as MethodCallExpression;

            if (methodCall == null)
            {
                throw new ArgumentOutOfRangeException("exp", "No valid expression found");
            }

            return ExamineMethods(obj, methodCall, PropName);
        }

        private Tuple<string, List<object>> ExamineMethods(T obj, MethodCallExpression methodCall, string propName)
        {
            var methodSignature = methodCall.Method.ToString();
            var variables = new List<object>();

            variables.AddRange(GetArgumentValue(obj, methodCall, methodCall.Arguments[1]));
                   
            variables.Add(propName);
            
            return new Tuple<string, List<object>>(_messageFormatString, variables);
        }        
    }
}
