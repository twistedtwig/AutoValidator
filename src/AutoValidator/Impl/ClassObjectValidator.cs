using System;
using System.Linq.Expressions;
using AutoValidator.Interfaces;

namespace AutoValidator.Impl
{
    public class ClassObjectValidator<T, TMember> : ClassObjectValidatorBase<T, TMember>
    {
        

        public ClassObjectValidator(Expression<Func<T, TMember>> memberSelectorExpression, Expression<Func<TMember, IValidatorExpression, bool>> memberValidationFunc) : base(memberSelectorExpression, memberValidationFunc)
        {

        }

        public ClassObjectValidator(Expression<Func<T, TMember>> memberSelectorExpression, Expression<Func<TMember, T, IValidatorExpression, bool>> memberValidationFunc) : base(memberSelectorExpression, memberValidationFunc)
        {

        }
    }
}
