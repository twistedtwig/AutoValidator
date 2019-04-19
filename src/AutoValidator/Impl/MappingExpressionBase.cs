using System;
using System.Collections.Generic;
using System.Linq;
using AutoValidator.Interfaces;
using AutoValidator.Models;

namespace AutoValidator.Impl
{
    public class MappingExpressionBase<T> : ITypeMapConfiguration
    {
        protected List<IObjectValidator<T>> Constraints;

        protected MappingExpressionBase()
        {
            Constraints = new List<IObjectValidator<T>>();
        }

        public Type SourceType => typeof(T);

        public ClassExpressionValidationResult ValidateExpression()
        {
            var result = new ClassExpressionValidationResult(typeof(T));

            var objType = typeof(T);
            var propNames = objType.GetProperties().Select(p =>  p.Name).ToList();

            var constraintPropNames = Constraints.Select(x => x.PropName).ToList();
            var constraintPropNameCount = constraintPropNames.GroupBy(s => s);

            foreach (var nameCount in constraintPropNameCount)
            {
                if (nameCount.Count() > 1)
                {
                    result.Errors.Add(new ExpressionValidationPropertyError(nameCount.Key, $"Duplicate mapping for property '{nameCount.Key}'"));
                }
            }

            var missingMappings = propNames.Except(constraintPropNames);
            foreach (var missingMapping in missingMappings)
            {
                result.Errors.Add(new ExpressionValidationPropertyError(missingMapping, $"Missing mapping for property '{missingMapping}'"));
            }

            result.Success = !result.Errors.Any();

            return result;
        }
    }
}
