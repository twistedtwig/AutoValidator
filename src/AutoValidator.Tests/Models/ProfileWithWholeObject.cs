using AutoValidator.Impl;

namespace AutoValidator.Tests.Models
{
    public class ProfileWithWholeObject : ClassValidationProfile
    {
        public ProfileWithWholeObject()
        {
            CreateMap<Model1>()
                .ForMember(x => x.Age, (age, obj, exp) => exp.MinValue(age, obj.Name.Length, null))
                .ForMember(x => x.Name, (name, obj, exp) => exp.Ignore());

            CreateMap<Model2>()
                .ForMember(x => x.Number, (num, obj, exp) => exp.MinValue(num, obj.EmailAddress.IndexOf("@"), "{2} should be at least {0}"))
                .ForMember(x => x.EmailAddress, (name, obj, exp) => exp.Ignore())
                .ForMember(x => x.Category, (name, obj, exp) => exp.Ignore());
        }
    }
}
