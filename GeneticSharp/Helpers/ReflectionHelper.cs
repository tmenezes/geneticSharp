using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using AutoBuilder;

namespace GeneticSharp.Helpers
{
    public static class ReflectionHelper
    {
        public static IEnumerable<PropertyInfo> GetProperties<T>()
        {
            return TypeManager.GetProperties(typeof(T));
        }


    }
}
