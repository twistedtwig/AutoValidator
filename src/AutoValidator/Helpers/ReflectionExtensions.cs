using System.Collections.Generic;
using System.Reflection;

namespace AutoValidator.Helpers
{
    //source: https://github.com/AutoMapper/AutoMapper/blob/master/src/AutoMapper/ReflectionExtensions.cs
    public static class ReflectionExtensions
    {
        public static IEnumerable<TypeInfo> GetDefinedTypes(this Assembly assembly) =>
            assembly.DefinedTypes;        
    }
}
