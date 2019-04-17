using System;
using System.Collections.Generic;
using AutoValidator.Interfaces;

namespace AutoValidator.Impl
{
    public class MappingExpressionBase<T> : ITypeMapConfiguration
    {
        protected List<IObjectValidator<T>> Constraints;

        protected MappingExpressionBase(Type source)
        {
            Constraints = new List<IObjectValidator<T>>();
            SourceType = source;
        }

        public Type SourceType { get; }
    }
}
