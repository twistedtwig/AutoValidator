using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using AutoValidator.Helpers;

namespace AutoValidator.Impl
{
    public abstract class ValidatorErrorMessageFactoryBase<T, TMember>
    {
        protected string PropName;

        public void SetPropName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException("name", "Propname must be provided");
            }
            PropName = name;
        }

        protected string GetArgumentMessageOverride(T obj, MethodCallExpression methodCall, int index)
        {
            var argValue = GetArgumentValue(obj, methodCall, methodCall.Arguments[index])[0];
            return argValue != null ? argValue.ToString() : string.Empty;
        }

        protected List<object> GetArgumentValue(T obj, MethodCallExpression wholeExpression, Expression methodExpression)
        {
            var visitor = new ValueExtractor<T, TMember>(obj, wholeExpression);
            visitor.Visit(methodExpression);

            var args = visitor.Arguments;
            return args;
        }
    }
}