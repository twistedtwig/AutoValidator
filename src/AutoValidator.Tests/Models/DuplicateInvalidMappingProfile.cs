using AutoValidator.Impl;

namespace AutoValidator.Tests.Models
{
    class DuplicateInvalidMappingProfile : ClassValidationProfile
    {
        public DuplicateInvalidMappingProfile()
        {
            CreateMap<Model1>()
                .ForMember(x => x.Age, (age, exp) => exp.MinValue(age, 18, null))
                .ForMember(x => x.Age, (age, exp) => exp.MinValue(age, 17, null))
                .ForMember(x => x.Name, (name, exp) => exp.NotNullOrEmpty(name, null));
        }
    }
}
