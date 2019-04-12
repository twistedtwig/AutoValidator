namespace AutoValidator.Interfaces
{
    public interface IClassValidationProfile
    {
        IMappingExpression<T> CreateMap<T>();
    }
}
