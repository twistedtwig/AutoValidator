using AutoValidator.Interfaces;
using AutoValidator.Models;

namespace AutoValidator.Impl
{
    public class ClassValidator<T> : IClassValidator<T> where T : class
    {
        private readonly IMappingExpression<T> _mappings;

        public ClassValidator(IMappingExpression<T> mappings)
        {
            _mappings = mappings;
        }

        public ValidationResult Validate(T item)
        {
            return _mappings.Validate(item);
        }
    }
}
