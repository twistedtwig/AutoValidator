using AutoValidator.Impl;

namespace DependencyInjectionExample.Models
{
    public class Model1Profile : ClassValidationProfile
    {
        public Model1Profile()
        {
            CreateMap<Model1>()
                .ForMember(x => x.Age, (age, exp) => exp.MinValue(age, 18, null))
                .ForMember(x => x.Name, (name, exp) => exp.NotNullOrEmpty(name, null))
                .ForMember(x => x.Category, (cat, exp) => exp.Ignore());
        }
    }
}
