using AutoValidator.Interfaces;
using AutoValidator.Models;

namespace AutoValidator.Impl
{
    public class ClassValidator<T> : IClassValidator<T> where T : class
    {
        private readonly IMappingExpression<T> _mappings;
        private readonly ValidatorSettings _settings;

        public ClassValidator(IMappingExpression<T> mappings, ValidatorSettings settings)
        {
            _mappings = mappings;
            _settings = settings;
        }

        public ValidationResult Validate(T item)
        {
            return _mappings.Validate(item, _settings);
        }
    }
}
