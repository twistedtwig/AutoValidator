namespace AutoValidator.Interfaces
{
    public interface IObjectValidator<T>
    {
        string ErrorMessage { get; }
        string PropName { get; }
        bool Validate(T obj);

    }
}
