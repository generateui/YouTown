using System;
using System.Collections.Generic;
using System.Linq;

namespace YouTown
{
    public interface IRandom
    {
        int NextInt(int min, int max);
    }

    public class DotNetRandom : IRandom
    {
        private readonly Random _random = new Random();
        public int NextInt(int min, int max)
        {
            return _random.Next(min, max);
        }
    }

    public static class RandomExtensions
    {
        public static T PickRandom<T>(this IReadOnlyList<T> list, IRandom random)
        {
            if (!list.Any())
            {
                throw new ArgumentException(nameof(list));
            }
            var randomIndex = random.NextInt(0, list.Count - 1);
            return list[randomIndex];
        }
    }
}
