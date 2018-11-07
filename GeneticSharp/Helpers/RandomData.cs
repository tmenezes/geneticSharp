using System;
using System.Collections.Generic;
using System.Text;

namespace GeneticSharp.Helpers
{
    public static class RandomData
    {
        private static readonly Random _random = new Random(DateTime.UtcNow.GetHashCode());

        public static int GetInt(int max) => _random.Next(max);
        public static bool GetBool() => _random.Next() % 2 == 0;
    }
}
