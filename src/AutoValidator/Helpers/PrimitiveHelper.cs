using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AutoValidator.Helpers
{
    //source: https://github.com/AutoMapper/AutoMapper/blob/master/src/AutoMapper/Configuration/Internal/PrimitiveHelper.cs
    public static class PrimitiveHelper
    {
        private static IEnumerable<MemberInfo> GetAllMembers(this Type type) =>
            type.GetTypeInheritance().Concat(type.GetTypeInfo().ImplementedInterfaces).SelectMany(i => i.GetDeclaredMembers());

        public static MemberInfo GetInheritedMember(this Type type, string name) => type.GetAllMembers().FirstOrDefault(mi => mi.Name == name);
        
        public static MemberInfo GetFieldOrProperty(Type type, string name)
            => type.GetInheritedMember(name) ?? throw new ArgumentOutOfRangeException(nameof(name), $"Cannot find member {name} of type {type}.");
    }
}