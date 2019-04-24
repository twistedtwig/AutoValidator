using System;

namespace AutoValidator.Interfaces
{
    public interface IConfigurationProvider
    {
        /// <summary>
        /// Create a configured factory that can create ready to go validators
        /// </summary>
        /// <returns></returns>
        IValidatorFactory CreateFactory();

        /// <summary>
        /// Create a function that will allow a factory to be created as and when wanted
        /// (useful for DI).
        /// </summary>
        /// <returns></returns>
        Func<IValidatorFactory> CreateFactoryFunc();

        /// <summary>
        /// Assert that all profile expressions are valid.
        /// </summary>
        void AssertExpressionsAreValid();
    }
}
