using System;
using System.Linq.Expressions;
using AutoValidator.Helpers;
using AutoValidator.Interfaces;

namespace AutoValidator.Impl
{
    public class ObjectValidator<T, TMember> : IObjectValidator<T>
    {
        private readonly Func<T, TMember> _memberSelector;
        private readonly Func<TMember, bool> _memberValidation;

        public string ErrorMessage { get; }
        public string PropName { get; }
        public string FunctionDescription { get; }

        public ObjectValidator(Expression<Func<T, TMember>> memberSelectorExpression, Func<TMember, bool> memberValidation, string errorMessage)
        {
            FunctionDescription = memberValidation.ToString();
            var memberInfo = ReflectionHelper.FindProperty(memberSelectorExpression);
            PropName = memberInfo.Name;
            _memberSelector = memberSelectorExpression.Compile();
            _memberValidation = memberValidation;
            ErrorMessage = errorMessage;
        }

        public bool Validate(T obj)
        {
            return _memberValidation.Invoke(_memberSelector.Invoke(obj));
        }
    }
}
