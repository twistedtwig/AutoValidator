using AutoValidator.Impl;

namespace AutoValidator.Tests.Models
{
    class NullItemMappingProfile : ClassValidationProfile
    {
        public NullItemMappingProfile()
        {
            CreateMap<NullableModel>()
                .ForMember(x => x.EmailAddress, (email, exp) => exp.IsNotNull(email, null))
                .ForMember(x => x.AreYouHappy, (happy, exp) => exp.IsNotNull(happy, null))
                .ForMember(x => x.Number, (num, exp) => exp.IsNotNull(num, null));
        }
    }
}
