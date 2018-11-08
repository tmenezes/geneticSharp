using System;
using System.Collections.Generic;
using System.Reflection;
using AutoBuilder;

namespace GeneticSharp.Helpers
{
    public static class ReflectionHelper
    {
        private static readonly Dictionary<Type, bool> _isCollection = new Dictionary<Type, bool>();

        public static IEnumerable<PropertyInfo> GetProperties<T>()
        {
            return TypeManager.GetProperties(typeof(T));
        }

        public static bool IsCollection(Type type)
        {
            if (!_isCollection.ContainsKey(type))
                _isCollection[type] = TypeManager.IsCollection(type);

            return _isCollection[type];
        }
    }
}
