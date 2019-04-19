using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace AutoValidator.Interfaces
{
    public interface IValidationExpressionErrorMessageFactory
    {
        Tuple<string, List<object>> Get<TMember>(Expression<Func<TMember, IValidatorExpression, bool>> exp, string propName);        
    }
}
