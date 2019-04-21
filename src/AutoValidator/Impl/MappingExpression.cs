using System;
using System.Linq.Expressions;
using AutoValidator.Interfaces;
using AutoValidator.Models;

namespace AutoValidator.Impl
{
    public class MappingExpression<T> : MappingExpressionBase<T>, IMappingExpression<T>
    {
        public ValidationResult Validate(T obj)
        {
            var result = ValidationResult.SuccessResult;

            foreach (var constraint in Constraints)
            {
                if(!constraint.Validate(obj))
                {
                    result.Success = false;
                    result.AddError(constraint.PropName, constraint.ErrorMessage);
                }
            }

            return result;
        }

        public IMappingExpression<T> ForMember<TMember>(Expression<Func<T, TMember>> memberSelectorExpression, Expression<Func<TMember, IValidatorExpression, bool>> memberValidationExpression)
        {
            Constraints.Add(new ClassObjectValidator<T, TMember>(memberSelectorExpression, memberValidationExpression));
            return this;
        }
        
        public IMappingExpression<T> ForMember<TMember>(Expression<Func<T, TMember>> memberSelectorExpression, Func<TMember, bool> memberValidationFunc, string errorMessage = null)
        {
            Constraints.Add(new ObjectValidator<T, TMember>(memberSelectorExpression, memberValidationFunc, errorMessage ?? memberSelectorExpression + " did not pass validation"));
            return this;
        }
    }
}
