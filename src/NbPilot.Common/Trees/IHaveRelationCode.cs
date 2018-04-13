using System;
using System.Collections.Generic;
using System.Linq;

namespace NbPilot.Common.Trees
{
    /// <summary>
    /// 拥有关系码
    /// 关系码 应该是 1. 1.21.2. 1.45.3.
    /// 最后加上.可以避免sql like选择问题
    /// </summary>
    public interface IHaveRelationCode
    {
        string RelationCode { get; set; }
    }

    public static class HaveRelationCodeExtensions
    {
        public static IEnumerable<T> OrderByRelationCode<T>(this IEnumerable<T> items) where T : IHaveRelationCode
        {
            return items.OrderBy(x => x.RelationCode, new FlatPositionComparer());
        }

        public static IEnumerable<T> OrderByRelationCodeDescending<T>(this IEnumerable<T> items) where T : IHaveRelationCode
        {
            return items.OrderByDescending(x => x.RelationCode, new FlatPositionComparer());
        }
        
        #region MyRegionGenerateNextRelationCode

        public static string GenerateNextRelationCode<T>(this IEnumerable<T> query, string parentRelationCode) where T : IHaveRelationCode
        {
            var parentRelationCodeFix = string.Empty;
            if (!string.IsNullOrWhiteSpace(parentRelationCode))
            {
                parentRelationCodeFix = parentRelationCode;
            }

            int directChildDotCount = 1;
            if (!string.IsNullOrWhiteSpace(parentRelationCodeFix))
            {
                directChildDotCount = parentRelationCodeFix.GetDotCount() + 1;
            }

            var directChildren = query.OrderByRelationCode().Where(x =>
                x.RelationCode.StartsWith(parentRelationCodeFix) &&
                x.RelationCode.GetDotCount() == directChildDotCount).ToList();
            var theLastOne = directChildren.LastOrDefault();
            if (theLastOne == null)
            {
                return parentRelationCodeFix + ".1";
            }

            var nextLastNum = theLastOne.RelationCode.FindLastDotNum() + 1;
            var nextChildCode = parentRelationCodeFix + "." + nextLastNum;
            return nextChildCode;
        }
        
        /// <summary>
        /// 获取当前关系码代表的最大树深度，默认基于1开始计算
        /// </summary>
        /// <param name="dtos"></param>
        /// <param name="firstDeep"></param>
        /// <returns></returns>
        public static int GetMaxDeep(this IEnumerable<IHaveRelationCode> dtos, int firstDeep = 1)
        {
            var maxCount = dtos.Max(x => x.RelationCode.GetDotCount());
            //e.g. maxCount = 3;
            //firstDeep = 1 => 3 + firstDeep - 1 => 3
            //firstDeep = 0 => 3 + firstDeep - 1 => 2

            ////1.1.
            //return maxCount + firstDeep - 1;

            //1.1
            return maxCount + firstDeep;
        }

        /// <summary>
        /// 获取当前关系码代表的树深度，默认基于1开始计算
        /// </summary>
        /// <param name="haveRelationCode"></param>
        /// <param name="firstDeep"></param>
        /// <returns></returns>
        public static int GetCurrentDeep(this IHaveRelationCode haveRelationCode, int firstDeep = 1)
        {
            var maxCount = haveRelationCode.RelationCode.GetDotCount();
            //e.g. maxCount = 3;
            //firstDeep = 1 => 3 + firstDeep - 1 => 3
            //firstDeep = 0 => 3 + firstDeep - 1 => 2

            ////1.1.
            //return maxCount + firstDeep - 1;

            //1.1
            return maxCount + firstDeep;
        }

        private static char _deepChar = '.';
        /// <summary>
        /// 获取当前关系码代表的关系层次数量（以.分隔）
        /// </summary>
        /// <param name="relationCode"></param>
        /// <returns></returns>
        public static int GetDotCount(this string relationCode)
        {
            return relationCode.ToCharArray().Count(c => c == _deepChar);
        }
        
        private static int FindLastDotNum(this string value)
        {
            var splits = value.Split('.').Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
            return int.Parse(splits.Last());
        }

        #endregion

    }
    public class FlatPositionComparer : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            if (x == y)
                return 0;

            // null == "before; "" == "0"
            x = x == null
                ? "before"
                : x.Trim().Length == 0 ? "0" : x.Trim(':').TrimEnd('.'); // ':' is _sometimes_ used as a partition identifier
            y = y == null
                ? "before"
                : y.Trim().Length == 0 ? "0" : y.Trim(':').TrimEnd('.');

            var xParts = x.Split(new[] { '.', ':' });
            var yParts = y.Split(new[] { '.', ':' });
            for (var i = 0; i < xParts.Count(); i++)
            {
                if (yParts.Length < i + 1) // x is further defined meaning it comes after y (e.g. x == 1.2.3 and y == 1.2)
                    return 1;

                int xPos;
                int yPos;
                var xPart = string.IsNullOrWhiteSpace(xParts[i]) ? "before" : xParts[i];
                var yPart = string.IsNullOrWhiteSpace(yParts[i]) ? "before" : yParts[i];

                xPart = NormalizeKnownPartitions(xPart);
                yPart = NormalizeKnownPartitions(yPart);

                var xIsInt = int.TryParse(xPart, out xPos);
                var yIsInt = int.TryParse(yPart, out yPos);

                if (!xIsInt && !yIsInt)
                    return string.Compare(string.Join(".", xParts), string.Join(".", yParts), StringComparison.OrdinalIgnoreCase);
                if (!xIsInt || (yIsInt && xPos > yPos)) // non-int after int or greater x pos than y pos (which is an int)
                    return 1;
                if (!yIsInt || xPos < yPos)
                    return -1;
            }

            if (xParts.Length < yParts.Length) // all things being equal y might be further defined than x (e.g. x == 1.2 and y == 1.2.3)
                return -1;

            return 0;
        }


        private static string NormalizeKnownPartitions(string partition)
        {
            if (partition.Length < 5) // known partitions are long
                return partition;

            if (string.Compare(partition, "before", StringComparison.OrdinalIgnoreCase) == 0)
                return "-9999";
            if (string.Compare(partition, "after", StringComparison.OrdinalIgnoreCase) == 0)
                return "9999";

            return partition;
        }
    }
}
