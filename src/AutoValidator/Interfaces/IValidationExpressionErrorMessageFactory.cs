using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace AutoValidator.Interfaces
{
    public interface IValidationExpressionErrorMessageFactory
    {
        Tuple<string, List<object>> Get<TMember>(Expression<Func<TMember, IValidatorExpression, bool>> exp, string propName);

        string Get(string value, string propName, object[] values);
    }
}
