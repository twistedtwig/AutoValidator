using AutoValidator.Impl;

namespace AutoValidator.Tests.Models
{
    public class MultipleMappingsProfile : ClassValidationProfile
    {
        public MultipleMappingsProfile()
        {
            CreateMap<Model1>()
                .ForMember(x => x.Age, (age, exp) => exp.MinValue(age, 18, null))
                .ForMember(x => x.Name, (name, exp) => exp.NotNullOrEmpty(name, null));

            CreateMap<Model2>()
                .ForMember(x => x.Category, (cat, exp) => exp.NotNullOrEmpty(cat, null))
                .ForMember(x => x.Number, (num, exp) => exp.MinValue(num, 5, null))
                .ForMember(x => x.EmailAddress, (email, exp) => exp.IsEmailAddress(email, null));
        }
    }
}
