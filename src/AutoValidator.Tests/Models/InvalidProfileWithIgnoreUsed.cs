﻿using AutoValidator.Impl;

namespace AutoValidator.Tests.Models
{
    public class InvalidProfileWithIgnoreUsed : ClassValidationProfile
    {
        public InvalidProfileWithIgnoreUsed()
        {
            CreateMap<Model1>()
                .ForMember(x => x.Age, (age, exp) => exp.MinValue(age, 18, null))
                .ForMember(x => x.Name, (name, exp) => exp.NotNullOrEmpty(name, null))
                .ForMember(x => x.Name, (name, exp) => exp.Ignore());
        }
    }
}
