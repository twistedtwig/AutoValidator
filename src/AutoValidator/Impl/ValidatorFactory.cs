using AutoValidator.Interfaces;

namespace AutoValidator.Impl
{
    public class ValidatorFactory : IValidatorFactory
    {
        public IValidator Create()
        {
            return new Validator();
        }

        public IClassValidator<T> Create<T>() where T : class
        {
            var validator = Create();

            //TODO how to create instance of class validator
            throw new System.NotImplementedException();
        }
    }
}


//need to think about how to do Validate<T>