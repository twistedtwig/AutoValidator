using AutoValidator.Impl;

namespace AutoValidator.Tests.Models
{
    public class MultipleMappingsWithErrorsProfile : ClassValidationProfile
    {
        public MultipleMappingsWithErrorsProfile()
        {
            CreateMap<Model1>()
                .ForMember(x => x.Age, (age, exp) => exp.MinValue(age, 18, null));

            CreateMap<Model2>()
                .ForMember(x => x.Category, (cat, exp) => exp.NotNullOrEmpty(cat, null))
                .ForMember(x => x.Category, (cat, exp) => exp.NotNullOrEmpty(cat, null))
                .ForMember(x => x.EmailAddress, (email, exp) => exp.IsEmailAddress(email, null));
        }
    }
}
