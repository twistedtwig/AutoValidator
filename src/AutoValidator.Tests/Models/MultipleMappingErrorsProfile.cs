using AutoValidator.Impl;

namespace AutoValidator.Tests.Models
{
    class MultipleMappingErrorsProfile : ClassValidationProfile
    {
        public MultipleMappingErrorsProfile()
        {
            CreateMap<Model1>()
                .ForMember(x => x.Age, (age, exp) => exp.MinValue(age, 18, null))
                .ForMember(x => x.Age, (age, exp) => exp.MinValue(age, 18, null));
        }
    }
}
