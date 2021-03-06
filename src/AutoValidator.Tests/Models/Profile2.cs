﻿using AutoValidator.Impl;

namespace AutoValidator.Tests.Models
{
    public class Profile2 : ClassValidationProfile
    {
        public Profile2()
        {
            CreateMap<Model1>()
                .ForMember(x => x.Age, (age, exp) => exp.MinValue(age, 18, null))
                .ForMember(x => x.Name, (name, exp) => exp.Ignore());
        }
    }
}
