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

            var constraintPropNames = Constraints.Select(x => x.PropName).Distinct().ToList();

            var constraintGroups = Constraints.GroupBy(c => c.PropName);
            foreach (var constraintGroup in constraintGroups)
            {
                var funcGroups = constraintGroup.Select(c => c.FunctionDescription).GroupBy(cg => cg);
                foreach (var funcGroup in funcGroups)
                {
                    if (funcGroup.Count() > 1)
                    {
                        result.Errors.Add(new ExpressionValidationPropertyError(constraintGroup.Key, $"Duplicate mapping for property '{constraintGroup.Key}'"));
                    }
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
