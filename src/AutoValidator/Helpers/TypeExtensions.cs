using System;
using System.Collections.Generic;
using System.Reflection;

namespace AutoValidator.Helpers
{
    //source: https://github.com/AutoMapper/AutoMapper/blob/master/src/AutoMapper/TypeExtensions.cs
    public static class TypeExtensions
    {       
        public static IEnumerable<MemberInfo> GetDeclaredMembers(this Type type) => type.GetTypeInfo().DeclaredMembers;

        public static IEnumerable<Type> GetTypeInheritance(this Type type)
        {
            yield return type;

            var baseType = type.BaseType();
            while (baseType != null)
            {
                yield return baseType;
                baseType = baseType.BaseType();
            }
        }
        
        public static Type BaseType(this Type type) => type.GetTypeInfo().BaseType;
    }
}
