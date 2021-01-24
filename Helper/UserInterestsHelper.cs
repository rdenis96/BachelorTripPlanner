using System;
using System.Collections.Generic;
using System.Linq;

namespace Helpers
{
    public static class UserInterestsHelper
    {
        public static string ConvertListToString(this List<string> list, char separator)
        {
            if (list == null)
                return string.Empty;

            return string.Join(separator, list.ToArray());
        }

        public static List<string> ConvertStringToList(this string str, char separator)
        {
            if (str == null || str == string.Empty)
                return new List<string>();

            return str.Split(separator).ToList();
        }

        public static List<T> Shuffle<T>(this IEnumerable<T> list)
        {
            if (list == null)
            {
                return new List<T>();
            }

            var castedList = list.ToList();

            Random random = new Random();
            int n = castedList.Count;

            for (int i = n - 1; i > 1; i--)
            {
                int rnd = random.Next(i + 1);

                T value = castedList[rnd];
                castedList[rnd] = castedList[i];
                castedList[i] = value;
            }

            return castedList;
        }
    }
}