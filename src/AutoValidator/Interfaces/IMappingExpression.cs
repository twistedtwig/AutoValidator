using System;
using System.Linq.Expressions;

namespace AutoValidator.Interfaces
{
    public interface ITypeMapConfiguration
    {
        Type SourceType { get; }
    }

    public interface IMappingExpression<T> : ITypeMapConfiguration
    {
        IMappingExpression<T> ForMember<TMember>(Expression<Func<T, TMember>> member, Action<IValidatorExpression> memberAction);
    }
}
