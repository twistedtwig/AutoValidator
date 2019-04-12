using AutoValidator.Models;

namespace AutoValidator.Interfaces
{
    public interface IClassValidator<T> where T : class
    {
        ValidationResult Validate(T item);
    }
}
