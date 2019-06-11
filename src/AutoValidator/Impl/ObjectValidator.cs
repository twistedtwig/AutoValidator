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
        private readonly Func<TMember, T, bool> _memberValidationWithT;

        public string ErrorMessage { get; private set; }
        public string PropName { get; }
        public string FunctionDescription { get; }
        private readonly IObjectValidatorErrorMessageFactory<T, TMember> _validationExpressionErrorMessageFactory;

        protected ObjectValidator()
        {
            _validationExpressionErrorMessageFactory = new ObjectValidatorErrorMessageFactory<T, TMember>();
        }

        public ObjectValidator(Expression<Func<T, TMember>> memberSelectorExpression, Expression<Func<TMember, bool>> memberValidation, string errorMessage) : this()
        {
            FunctionDescription = memberValidation.ToString();
            var memberInfo = ReflectionHelper.FindProperty(memberSelectorExpression);
            PropName = memberInfo.Name;
            _memberSelector = memberSelectorExpression.Compile();
            _memberValidation = memberValidation.Compile();
            ErrorMessage = errorMessage;

            _validationExpressionErrorMessageFactory.SetPropName(PropName);
            _validationExpressionErrorMessageFactory.SetErrorFormat(errorMessage);
            _validationExpressionErrorMessageFactory.SetupExpression(memberValidation);
        }

        public ObjectValidator(Expression<Func<T, TMember>> memberSelectorExpression, Expression<Func<TMember, T, bool>> memberValidation, string errorMessage) : this()
        {
            FunctionDescription = memberValidation.ToString();
            var memberInfo = ReflectionHelper.FindProperty(memberSelectorExpression);
            PropName = memberInfo.Name;
            _memberSelector = memberSelectorExpression.Compile();
            _memberValidationWithT = memberValidation.Compile();
            ErrorMessage = errorMessage;

            _validationExpressionErrorMessageFactory.SetPropName(PropName);
            _validationExpressionErrorMessageFactory.SetErrorFormat(errorMessage);
            _validationExpressionErrorMessageFactory.SetupExpression(memberValidation);
        }

        public bool Validate(T obj)
        {
            var member = _memberSelector.Invoke(obj);
            var validate = _memberValidation != null 
                ? _memberValidation.Invoke(member) 
                : _memberValidationWithT.Invoke(member, obj);

            if (!validate)
            {
                var formattingInfo = _validationExpressionErrorMessageFactory.Invoke(obj);
                var ErrorMessageFormat = formattingInfo.Item1;
                var FormatObjectArgs = formattingInfo.Item2;

                FormatObjectArgs.Add(member);

                ErrorMessage = string.Format(ErrorMessageFormat, FormatObjectArgs.ToArray());
            }
            else
            {
                ErrorMessage = string.Empty;
            }

            return validate;
        }
    }
}
