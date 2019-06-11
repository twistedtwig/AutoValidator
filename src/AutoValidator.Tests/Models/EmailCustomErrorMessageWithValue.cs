using AutoValidator.Impl;

namespace AutoValidator.Tests.Models
{
    public class EmailCustomErrorMessageWithValue : ClassValidationProfile
    {
        public EmailCustomErrorMessageWithValue()
        {
            CreateMap<Model2>()
                .ForMember(x => x.Category, (cat, exp) => exp.NotNullOrEmpty(cat, null))
                .ForMember(x => x.Number, (num, exp) => exp.MinValue(num, 5, null))
                .ForMember(x => x.EmailAddress, (email, exp) => exp.IsEmailAddress(email, "{1} is not a valid email address"));
        }
    }
}
