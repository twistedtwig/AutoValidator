using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoValidator.Helpers;
using AutoValidator.Interfaces;
using AutoValidator.Models;

namespace AutoValidator.Impl
{
    public class MapperConfigurationExpression : IMapperConfigurationExpression
    {
        private readonly IList<IClassValidationProfile> _profiles = new List<IClassValidationProfile>();

        public IEnumerable<IClassValidationProfile> Profiles => _profiles;

        public void AddProfile(IClassValidationProfile profile)
        {
            var newProfileType = profile.GetType();
            if (_profiles.All(p => p.GetType() != newProfileType))
            {
                _profiles.Add(profile);
            }
        }

        public void AddProfile<TProfile>() where TProfile : IClassValidationProfile, new() => AddProfile(new TProfile());

        public void AddProfile(Type profileType) => AddProfile((IClassValidationProfile)Activator.CreateInstance(profileType));


        public void AddProfile(Assembly assemblyToScan) => AddProfiles(new[] { assemblyToScan });

        public void AddProfiles(IEnumerable<Assembly> assembliesToScan) => AddMaps(assembliesToScan);

        public List<ProfileExpressionValidationResult> GetConfigurationExpressionValidation()
        {
            var list = new List<ProfileExpressionValidationResult>();
            foreach (var profile in _profiles)
            {
                list.Add(profile.ValidateExpression());
            }

            return list;
        }


        private void AddMaps(IEnumerable<Assembly> assembliesToScan)
            => AddMapsCore(assembliesToScan);





        private void AddMapsCore(IEnumerable<Assembly> assembliesToScan)
        {
            var allTypes = assembliesToScan.Where(a => !a.IsDynamic && a != typeof(IClassValidationProfile).Assembly).SelectMany(a => a.GetDefinedTypes()).ToArray();

            foreach (var type in allTypes)
            {
                if (typeof(IClassValidationProfile).IsAssignableFrom(type) && !type.IsAbstract)
                {
                    AddProfile(type.AsType());
                }
            }
        }
    }
}
