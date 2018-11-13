using System;
using System.Collections.Generic;

namespace GeneticSharp.Helpers
{
    internal static class NumberHelper
    {
        internal static bool TryGetValue(object input, out decimal value)
        {
            try
            {
                value = Convert.ToDecimal(input);
                return true;
            }
            catch (Exception)
            {
                value = 0;
                return false;
            }
        }

        internal static object CastValueToProperType(Type type, decimal value)
        {
            var transformers = new Dictionary<Type, Func<decimal, object>>()
            {
                { typeof(int), v => (int)v },
                { typeof(byte), v => (byte)v },
                { typeof(short), v => (short)v },
                { typeof(char), v => (char)v },
                { typeof(long), v => (long)v },
                { typeof(float), v => (float)v },
                { typeof(double), v => (double)v },
            };

            var realType = GetTypeOrUnderlyingType(type);

            return transformers.ContainsKey(realType)
                       ? transformers[realType].Invoke(value)
                       : value;
        }


        private static Type GetTypeOrUnderlyingType(Type type)
        {
            return ReflectionHelper.IsCollection(type) ? ReflectionHelper.GetCollectionItemType(type) : type;
        }
    }
}
