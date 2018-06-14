using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable once CheckNamespace
namespace ZQNB.Common
{
    public static class SimpleMapToExtensions
    {
        /// <summary>
        /// 转换成映射对象(Auto Null)
        /// </summary>
        /// <typeparam name="TMapTo"></typeparam>
        /// <param name="mapFrom"></param>
        /// <param name="excludeProperties"></param>
        /// <returns></returns>
        public static TMapTo TryToMapped<TMapTo>(this object mapFrom, params string[] excludeProperties) where TMapTo : new()
        {
            if (mapFrom == null)
            {
                return default(TMapTo);
            }
            return ToMapped<TMapTo>(mapFrom, excludeProperties);
        }
        
        /// <summary>
        /// 转换成映射对象的枚举(Exclude Null)
        /// </summary>
        /// <typeparam name="TMapTo"></typeparam>
        /// <param name="froms"></param>
        /// <param name="excludeProperties"></param>
        /// <returns></returns>
        public static IEnumerable<TMapTo> TryToMappedEnumerable<TMapTo>(this IEnumerable froms, params string[] excludeProperties) where TMapTo : new()
        {
            foreach (var @from in froms)
            {
                var temp = TryToMapped<TMapTo>(@from);
                if (temp != null)
                {
                    yield return temp;
                }
            }
        }

        /// <summary>
        /// 转换成映射对象的List(Exclude Null)
        /// </summary>
        /// <typeparam name="MapTo"></typeparam>
        /// <param name="froms"></param>
        /// <param name="excludeProperties"></param>
        /// <returns></returns>
        public static IList<MapTo> TryToMappedList<MapTo>(this IEnumerable froms, params string[] excludeProperties) where MapTo : new()
        {
            var mappedEnumerable = TryToMappedEnumerable<MapTo>(froms, excludeProperties);
            return mappedEnumerable.ToList();
        }


        /// <summary>
        /// 转换成映射对象
        /// </summary>
        /// <typeparam name="TMapTo"></typeparam>
        /// <param name="mapFrom"></param>
        /// <param name="excludeProperties"></param>
        /// <returns></returns>
        public static TMapTo ToMapped<TMapTo>(this object mapFrom, params string[] excludeProperties) where TMapTo : new()
        {
            throw new NotImplementedException();
            //var orgDto = new TMapTo();
            ////todo better relace impl from reflection with expression tree
            //MyModelHelper.TryCopyProperties(orgDto, mapFrom, excludeProperties);
            //return orgDto;
        }

        /// <summary>
        /// 转换成映射对象的枚举
        /// </summary>
        /// <typeparam name="TMapTo"></typeparam>
        /// <param name="froms"></param>
        /// <param name="excludeProperties"></param>
        /// <returns></returns>
        public static IEnumerable<TMapTo> ToMappedEnumerable<TMapTo>(this IEnumerable froms, params string[] excludeProperties) where TMapTo : new()
        {
            throw new NotImplementedException();
            //return from object mapFrom in froms select ToMapped<TMapTo>(mapFrom, excludeProperties);
        }

        /// <summary>
        /// 转换成映射对象的List
        /// </summary>
        /// <typeparam name="MapTo"></typeparam>
        /// <param name="froms"></param>
        /// <param name="excludeProperties"></param>
        /// <returns></returns>
        public static IList<MapTo> ToMappedList<MapTo>(this IEnumerable froms, params string[] excludeProperties) where MapTo : new()
        {
            throw new NotImplementedException();
            //var mappedEnumerable = ToMappedEnumerable<MapTo>(froms, excludeProperties);
            //return mappedEnumerable.ToList();
        }
    }
}
