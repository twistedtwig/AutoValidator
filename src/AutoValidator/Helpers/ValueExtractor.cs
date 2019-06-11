using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AutoValidator.Helpers
{
    internal class ValueExtractor<T, TMember> : ExpressionVisitor
    {
        private readonly Dictionary<Type, Dictionary<string, object>> anonymousFields;
        private readonly T _item;
        private readonly MethodCallExpression _wholeExpression;

        public ValueExtractor(T item, MethodCallExpression expression)
        {
            Arguments = new List<object>();
            anonymousFields = new Dictionary<Type, Dictionary<string, object>>();

            _item = item;
            _wholeExpression = expression;
        }

        public List<object> Arguments { get; }

        protected override Expression VisitMember(MemberExpression node)
        {
            var memberName = node.Member.Name;
            var type = node.Member.DeclaringType;

            var memberType = node.NodeType;
            if (memberType == ExpressionType.MemberAccess)
            {
                if (node.Expression is MemberExpression memberExpression)
                {
                    if (memberExpression.Expression is ParameterExpression secondLevelExpression)
                    {
                        var secondLevelMember = memberExpression.Member;

                        var parameter = Expression.Parameter(typeof(T), secondLevelExpression.Name);
                        var propExpression = Expression.PropertyOrField(parameter, typeof(T).GetProperty(secondLevelMember.Name).Name);

                        var compiledExpression = Expression.Lambda<Func<T, object>>(propExpression, parameter).Compile();
                        var result = compiledExpression.Invoke(_item);

                        var value = result.GetType().GetProperty(memberName).GetValue(result);
                        Arguments.Add(value);
                    }
                }
            }

            var baseResult = base.VisitMember(node);

            if (anonymousFields.ContainsKey(type))
            {
                Arguments.Add(anonymousFields[type][memberName]);
            }

            return baseResult;
        }

        protected override Expression VisitMethodCall(MethodCallExpression expression)
        {
            var arguments = expression.Arguments;
            var method = expression.Method;
            if (expression.Object is MemberExpression propToCallMethodOn)
            {
                if (propToCallMethodOn.Expression is ParameterExpression propObjExpression)
                {
                    ProcessParameterExpressionToGetArgument(propObjExpression, propToCallMethodOn, method, arguments);
                }

                if (propToCallMethodOn.Expression.NodeType == ExpressionType.MemberAccess)
                {
                    ProcessMemberAccessToGetArgument(propToCallMethodOn);
                }
            }

            return base.VisitMethodCall(expression);
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            var constantType = node.Type;
            if (constantType == typeof(int) || constantType == typeof(string)) // and so on
            {
                Arguments.Add(node.Value);
            }
            else if (IsAnonymousType(constantType) && !anonymousFields.ContainsKey(constantType))
            {
                var fields = new Dictionary<string, object>();
                anonymousFields.Add(constantType, fields);

                foreach (var field in constantType.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.GetField))
                    fields.Add(field.Name, field.GetValue(node.Value));
            }

            return base.VisitConstant(node);
        }

        private void ProcessMemberAccessToGetArgument(MemberExpression propToCallMethodOn)
        {
            var memberProperty = propToCallMethodOn.Member;
            var propAccessExpression = propToCallMethodOn.Expression as MemberExpression;
            var accessName = propAccessExpression.Member.Name;

            if (propAccessExpression.Expression is ParameterExpression propAccessObjExpression)
            {
                var objExpression = Expression.Parameter(typeof(T), propAccessObjExpression.Name);
                var objFieldExpression = Expression.PropertyOrField(objExpression, typeof(T).GetProperty(accessName).Name);
                var value = Expression.Lambda(objFieldExpression, objExpression).Compile().DynamicInvoke(_item);

                if (value != null)
                {
                    var result = value.GetType().GetProperty(memberProperty.Name).GetValue(value, null);
                    if (result != null)
                    {
                        Arguments.Add(result);
                    }
                }
            }
        }

        private void ProcessParameterExpressionToGetArgument(ParameterExpression propObjExpression, MemberExpression propToCallMethodOn, MethodInfo method, ReadOnlyCollection<Expression> arguments)
        {
            var propName = propObjExpression.Name;
            var propMemberName = propToCallMethodOn.Member.Name;

            var parameter = Expression.Parameter(typeof(T), propName);
            var propExpression = Expression.PropertyOrField(parameter, typeof(T).GetProperty(propMemberName).Name);

            var value = Expression.Lambda(propExpression, parameter).Compile().DynamicInvoke(_item);

            var result = method.Invoke(value, arguments.Cast<ConstantExpression>().Select(a => a.Value).ToArray());
            if (result != null)
            {
                Arguments.Add(result);
            }
        }

        private static bool IsAnonymousType(Type type)
        {
            var hasSpecialChars = type.Name.Contains("<") || type.Name.Contains(">");
            return hasSpecialChars && type.GetCustomAttributes(typeof(CompilerGeneratedAttribute), inherit: false).Any();
        }
    }
}
