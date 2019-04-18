using System;
using System.Linq.Expressions;
using AutoValidator.Models;

namespace AutoValidator.Interfaces
{
    public interface ITypeMapConfiguration
    {
        ConfigurationClassExpressionValidationResult ValidateExpression();
    }

    public interface IMappingExpression<T> : ITypeMapConfiguration
    {
        ValidationResult Validate(T obj);

        /// <summary>
        /// Standard validation expressions
        /// </summary>
        /// <typeparam name="TMember"></typeparam>
        /// <param name="memberSelectorExpression"></param>
        /// <param name="memberValidation"></param>
        /// <returns></returns>
        IMappingExpression<T> ForMember<TMember>(Expression<Func<T, TMember>> memberSelectorExpression, Expression<Func<TMember, IValidatorExpression, bool>> memberValidationExpression);

        /// <summary>
        /// Custom Mapping expression.
        ///
        /// Allows user to define any boolean func they want to evaluate
        /// </summary>
        /// <typeparam name="TMember"></typeparam>
        /// <param name="memberSelectorExpression"></param>
        /// <param name="memberValidationFunc"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        IMappingExpression<T> ForMember<TMember>(Expression<Func<T, TMember>> memberSelectorExpression, Func<TMember, bool> memberValidationFunc, string errorMessage = null);
    }
}
