using System;
using System.Linq.Expressions;
using AutoValidator.Models;

namespace AutoValidator.Interfaces
{
    public interface ITypeMapConfiguration
    {
        Type SourceType { get; }
        ClassExpressionValidationResult ValidateExpression();
    }

    public interface IMappingExpression<T> : ITypeMapConfiguration
    {
        ValidationResult Validate(T obj, ValidatorSettings settings);

        /// <summary>
        /// Standard validation expressions
        /// </summary>
        /// <typeparam name="TMember">Property type to be validated</typeparam>
        /// <param name="memberSelectorExpression">Expression to select the property to be validated</param>
        /// <param name="memberValidationExpression">expression used to validate the property</param>
        /// <returns></returns>
        IMappingExpression<T> ForMember<TMember>(Expression<Func<T, TMember>> memberSelectorExpression, Expression<Func<TMember, IValidatorExpression, bool>> memberValidationExpression);

        /// <summary>
        /// Standard validation expressions
        /// <para>
        /// Also allows the whole model to be passed in to the expression
        /// </para>
        /// </summary>
        /// <typeparam name="TMember">Property type to be validated</typeparam>
        /// <param name="memberSelectorExpression">Expression to select the property to be validated</param>
        /// <param name="memberValidationExpression">expression used to validate the property</param>
        /// <returns></returns>
        IMappingExpression<T> ForMember<TMember>(Expression<Func<T, TMember>> memberSelectorExpression, Expression<Func<TMember, T, IValidatorExpression, bool>> memberValidationExpression);

        /// <summary>
        /// Custom Mapping expression.
        ///
        /// Allows user to define any boolean func they want to evaluate
        /// </summary>
        /// <typeparam name="TMember">Property type to be validated</typeparam>
        /// <param name="memberSelectorExpression">expression to select the member / property to be evaluated</param>
        /// <param name="memberValidationFunc">Custom function to validate the member.  Takes TMember, must return bool</param>
        /// <param name="errorMessage">error message string format</param>
        /// <returns></returns>
        IMappingExpression<T> ForMember<TMember>(Expression<Func<T, TMember>> memberSelectorExpression, Expression<Func<TMember, bool>> memberValidationFunc, string errorMessage = null);

        /// <summary>
        /// Custom Mapping expression.
        ///
        /// Allows user to define any boolean func they want to evaluate
        /// <para>
        /// Also allows the whole model to be passed in to the expression
        /// </para>
        /// </summary>
        /// <typeparam name="TMember">Property type to be validated</typeparam>
        /// <param name="memberSelectorExpression">expression to select the member / property to be evaluated</param>
        /// <param name="memberValidationFunc">Custom function to validate the member.  Takes TMember and T, must return bool</param>
        /// <param name="errorMessage">error message string format</param>
        /// <returns></returns>
        IMappingExpression<T> ForMember<TMember>(Expression<Func<T, TMember>> memberSelectorExpression, Expression<Func<TMember, T, bool>> memberValidationFunc, string errorMessage = null);
    }
}
