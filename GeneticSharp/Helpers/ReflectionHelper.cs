using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using AutoBuilder;

namespace GeneticSharp.Helpers
{
    public static class ReflectionHelper
    {
        private static readonly ConcurrentDictionary<Type, bool> _isCollection = new ConcurrentDictionary<Type, bool>();

        public static IEnumerable<PropertyInfo> GetProperties<T>()
        {
            return TypeManager.GetProperties(typeof(T));
        }

        public static bool IsCollection(PropertyInfo property) => IsCollection(property.PropertyType);

        public static bool IsCollection(Type type) => _isCollection.GetOrAdd(type, TypeManager.IsCollection);
    }
}
