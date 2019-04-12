namespace AutoValidator.Interfaces
{
    public interface IValidatorFactory
    {
        IValidator Create();
        IClassValidator<T> Create<T>() where T : class;
    }
}
