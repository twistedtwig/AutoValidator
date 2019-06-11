namespace AutoValidator.Interfaces
{
    public interface IMemberValidationFunc<T, TMember>
    {
        bool Invoke(T obj, TMember member, IValidatorExpression validatorExpression);
    }
}
