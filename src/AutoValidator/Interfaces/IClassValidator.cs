using AutoValidator.Models;

namespace AutoValidator.Interfaces
{
    public interface IClassValidator<T> where T : class
    {
        /// <summary>
        /// will Validate the given object of type T.
        /// Schema validation should have already been setup
        /// </summary>
        /// <param name="item">The object to be validated</param>
        /// <returns></returns>
        ValidationResult Validate(T item);
    }
}
