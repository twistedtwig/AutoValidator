using AutoValidator.Impl;

namespace AutoValidator.Tests.Models
{
    class DuplicateMappingProfile : ClassValidationProfile
    {
        public DuplicateMappingProfile()
        {
            CreateMap<Model1>()
                .ForMember(x => x.Age, (age, exp) => exp.MinValue(age, 18, null))
                .ForMember(x => x.Name, (name, exp) => exp.NotNullOrEmpty(name, null))
                .ForMember(x => x.Name, (name, exp) => exp.MinLength(name, 3, null))
                .ForMember(x => x.Name, (name, exp) => exp.MaxLength(name, 13, null));
        }
    }
}
