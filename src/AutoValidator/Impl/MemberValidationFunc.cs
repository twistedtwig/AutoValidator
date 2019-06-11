using System;
using AutoValidator.Interfaces;

namespace AutoValidator.Impl
{
    public class MemberValidationFunc<T, TMember> : IMemberValidationFunc<T, TMember>
    {
        private readonly Func<TMember, IValidatorExpression, bool> _memberValidation;

        public MemberValidationFunc(Func<TMember, IValidatorExpression, bool> func)
        {
            _memberValidation = func;
        }

        public bool Invoke(T obj, TMember member, IValidatorExpression validatorExpression)
        {
            return _memberValidation.Invoke(member, validatorExpression);
        }
    }

    public class MemberAndClassValidationFunc<T, TMember> : IMemberValidationFunc<T, TMember>
    {
        private readonly Func<TMember, T, IValidatorExpression, bool> _memberValidation;

        public MemberAndClassValidationFunc(Func<TMember, T, IValidatorExpression, bool> func)
        {
            _memberValidation = func;
        }

        public bool Invoke(T obj, TMember member, IValidatorExpression validatorExpression)
        {
            return _memberValidation.Invoke(member, obj, validatorExpression);
        }
    }
}
