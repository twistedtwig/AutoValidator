using System;
using System.Linq.Expressions;
using AutoValidator.Helpers;
using AutoValidator.Interfaces;

namespace AutoValidator.Impl
{
    public class MappingExpression<T> : MappingExpressionBase, IMappingExpression<T>
    {
        public MappingExpression() : base(typeof(T)) { }

        public IMappingExpression<T> ForMember<TMember>(Expression<Func<T, TMember>> member, Action<IValidatorExpression> memberAction)
        {
            var memberInfo = ReflectionHelper.FindProperty(member);

            return this;
        }
    }
}
