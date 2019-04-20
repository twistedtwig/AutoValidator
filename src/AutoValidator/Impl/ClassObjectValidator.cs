using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using AutoValidator.Helpers;
using AutoValidator.Interfaces;

namespace AutoValidator.Impl
{
    public class ClassObjectValidator<T, TMember> : IObjectValidator<T>
    {
        private readonly Func<T, TMember> _memberSelector;
        private readonly Func<TMember, IValidatorExpression, bool> _memberValidation;
        private readonly IValidatorExpression _validatorExpression;

        public string PropName { get; }
        public string FunctionDescription { get; }
        public string ErrorMessage { get; private set; }

        private readonly string _errorMessageFormat;
        private readonly List<Object> _formatObjectArgs;

        public ClassObjectValidator(Expression<Func<T, TMember>> memberSelectorExpression, Expression<Func<TMember, IValidatorExpression, bool>> memberValidationFunc)
        {
            var memberExp = memberValidationFunc;
            var funcStr = memberValidationFunc.Body.ToString();
            FunctionDescription = funcStr.Substring(0, funcStr.IndexOf("("));

            var memberInfo = ReflectionHelper.FindProperty(memberSelectorExpression);
            PropName = memberInfo.Name;
            _memberSelector = memberSelectorExpression.Compile();
            _memberValidation = memberValidationFunc.Compile();

            _validatorExpression = new ClassValidatorExpression();
            var errorMessageFactory = new ValidationExpressionErrorMessageFactory();

            var formattingInfo = errorMessageFactory.Get(memberExp, PropName);
            _errorMessageFormat = formattingInfo.Item1;
            _formatObjectArgs = formattingInfo.Item2;
        }

        public bool Validate(T obj)
        {
            TMember member = _memberSelector.Invoke(obj);
            var validate = _memberValidation.Invoke(member, _validatorExpression);

            if (!validate)
            {
                _formatObjectArgs.Add(member);

                ErrorMessage = string.Format(_errorMessageFormat, _formatObjectArgs.ToArray());
            }
            else
            {
                ErrorMessage = string.Empty;
            }

            return validate;
        }
    }
}
