using System.Collections.Generic;
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

        public ConfigurationClassExpressionValidationResult ValidateExpression()
        {
            // we are checking that all properties have one and only one mapping

            throw new System.NotImplementedException();
        }
    }
}
