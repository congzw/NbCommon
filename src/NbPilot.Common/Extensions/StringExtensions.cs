using System;
using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace NbPilot.Common
{
    public static class StringExtensions
    {
        public static string JoinToString<T>(this IEnumerable<T> values, string separator = ",")
        {
            if (values == null)
            {
                return string.Empty;
            }
            return string.Join(separator, values);
        }
        
        /// <summary>
        /// 宽松的字符串比较，如果双方为空或null都视为相等
        /// </summary>
        /// <param name="value"></param>
        /// <param name="value2"></param>
        /// <param name="stringComparison"></param>
        /// <returns></returns>
        public static bool NbEquals(this string value, string value2, StringComparison stringComparison = StringComparison.OrdinalIgnoreCase)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return string.IsNullOrWhiteSpace(value2);
            }

            return value.Trim().Equals(value2.Trim(), stringComparison);
        }
    }
}
