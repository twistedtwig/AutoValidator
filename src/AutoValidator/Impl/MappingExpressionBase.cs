using System;
using AutoValidator.Interfaces;

namespace AutoValidator.Impl
{
    public class MappingExpressionBase : ITypeMapConfiguration
    {
        protected MappingExpressionBase(Type source)
        {
            SourceType = source;
        }

        public Type SourceType { get; }
    }
}
