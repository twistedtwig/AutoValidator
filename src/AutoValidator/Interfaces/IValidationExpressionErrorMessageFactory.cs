﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace AutoValidator.Interfaces
{
    public interface IValidationExpressionErrorMessageFactory<T, TMember>
    {
        /// <summary>
        /// Set the property name to be evaluated for error messages
        /// </summary>
        /// <param name="name"></param>
        void SetPropName(string name);

        /// <summary>
        /// set the expression to be used to evaluate the and property
        /// </summary>
        /// <param name="exp"></param>
        void SetupExpression(Expression<Func<TMember, IValidatorExpression, bool>> exp);

        /// <summary>
        /// set the expression to be used to evaluate the object and the property
        /// </summary>
        /// <param name="exp"></param>
        void SetupExpression(Expression<Func<TMember, T, IValidatorExpression, bool>> exp);
        
        /// <summary>
        /// Evaluate the property with the propname and expression previously set.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        Tuple<string, List<object>> Invoke(T obj);
    }
}
