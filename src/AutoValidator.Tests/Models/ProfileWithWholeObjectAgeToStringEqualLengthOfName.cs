using AutoValidator.Impl;

namespace AutoValidator.Tests.Models
{
    public class ProfileWithWholeObjectAgeToStringEqualLengthOfName : ClassValidationProfile
    {
        public ProfileWithWholeObjectAgeToStringEqualLengthOfName()
        {
            CreateMap<Model1>()
                .ForMember(x => x.Age, (age, obj, exp) => exp.MinValue(age, obj.Age.ToString() == obj.Name.Length.ToString() ? 1 : 99, "{5} should be at least {1}"))
                .ForMember(x => x.Name, (name, obj, exp) => exp.Ignore());
        }
    }
}
