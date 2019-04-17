using AutoValidator.Impl;

namespace AutoValidator.Tests.Models
{
    class MissingMappingProfile : ClassValidationProfile
    {
        public MissingMappingProfile()
        {
            CreateMap<Model1>()
                .ForMember(x => x.Age, (age, exp) => exp.MinValue(age, 18, null));
        }
    }
}
