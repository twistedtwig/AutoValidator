using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using AutoValidator.Helpers;
using AutoValidator.Interfaces;

namespace AutoValidator.Impl
{
    public abstract class ClassObjectValidatorBase<T, TMember> : IObjectValidator<T>
    {
        public string PropName { get; private set; }
        public string FunctionDescription { get; private set; }
        public string ErrorMessage { get; protected set; }

        protected Func<T, TMember> MemberSelector { get; private set; }
        protected IValidatorExpression ValidatorExpression { get; private set; }
        
        protected string ErrorMessageFormat { get; private set; }
        protected List<Object> FormatObjectArgs { get; private set; }

        private IMemberValidationFunc<T, TMember> _memberValidation;
        private readonly IValidationExpressionErrorMessageFactory<T, TMember> _validationExpressionErrorMessageFactory;


        private void ExamineMemberSelectorExpression(Expression<Func<T, TMember>> memberSelectorExpression)
        {
            var memberInfo = ReflectionHelper.FindProperty(memberSelectorExpression);
            PropName = memberInfo.Name;
            MemberSelector = memberSelectorExpression.Compile();
        }

        private void ExamineMemberValidation(Expression<Func<TMember, IValidatorExpression, bool>> memberValidationFunc)
        {
            var funcStr = memberValidationFunc.Body.ToString();
            FunctionDescription = funcStr.Substring(0, funcStr.IndexOf("("));

            _memberValidation = new MemberValidationFunc<T, TMember>(memberValidationFunc.Compile());            
        }

        private void ExamineMemberValidation(Expression<Func<TMember, T, IValidatorExpression, bool>> memberValidationFunc)
        {
            var memberExp = memberValidationFunc;
            var funcStr = memberValidationFunc.Body.ToString();
            FunctionDescription = funcStr.Substring(0, funcStr.IndexOf("("));

            _memberValidation = new MemberAndClassValidationFunc<T, TMember>(memberValidationFunc.Compile());
        }
        
        protected ClassObjectValidatorBase()
        {
            ValidatorExpression = new ClassValidatorExpression();
            _validationExpressionErrorMessageFactory = new ValidationExpressionErrorMessageFactory<T, TMember>();
        }

        protected ClassObjectValidatorBase(Expression<Func<T, TMember>> memberSelectorExpression, Expression<Func<TMember, IValidatorExpression, bool>> memberValidationFunc) : this()
        {
            ExamineMemberSelectorExpression(memberSelectorExpression);
            ExamineMemberValidation(memberValidationFunc);
            _validationExpressionErrorMessageFactory.SetPropName(PropName);
            _validationExpressionErrorMessageFactory.SetupExpression(memberValidationFunc);
        }

        protected ClassObjectValidatorBase(Expression<Func<T, TMember>> memberSelectorExpression, Expression<Func<TMember, T, IValidatorExpression, bool>> memberValidationFunc) : this()
        {
            ExamineMemberSelectorExpression(memberSelectorExpression);
            ExamineMemberValidation(memberValidationFunc);
            _validationExpressionErrorMessageFactory.SetPropName(PropName);
            _validationExpressionErrorMessageFactory.SetupExpression(memberValidationFunc);
        }

        public bool Validate(T obj)
        {
            TMember member = MemberSelector.Invoke(obj);
            var validate = _memberValidation.Invoke(obj, member, ValidatorExpression);

            if (!validate)
            {
                var formattingInfo = _validationExpressionErrorMessageFactory.Invoke(obj);
                ErrorMessageFormat = formattingInfo.Item1;
                FormatObjectArgs = formattingInfo.Item2;

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
