using System;
using System.Collections.Generic;
using System.Linq;

namespace NbPilot.Common
{
    public class GuidHelper
    {
        /// <summary>
        /// Generate Guid By Order
        /// </summary>
        /// <returns></returns>
        public static Guid GenerateComb()
        {
            byte[] guidArray = Guid.NewGuid().ToByteArray();

            DateTime baseDate = new DateTime(1900, 1, 1);
            DateTime now = DateTime.Now;

            // Get the days and milliseconds which will be used to build the byte string
            TimeSpan days = new TimeSpan(now.Ticks - baseDate.Ticks);
            TimeSpan msecs = now.TimeOfDay;

            // Convert to a byte array
            // Note that SQL Server is accurate to 1/300th of a millisecond so we divide by 3.333333
            byte[] daysArray = BitConverter.GetBytes(days.Days);
            byte[] msecsArray = BitConverter.GetBytes((long)(msecs.TotalMilliseconds / 3.333333));

            // Reverse the bytes to match SQL Servers ordering
            Array.Reverse(daysArray);
            Array.Reverse(msecsArray);

            // Copy the bytes into the guid
            Array.Copy(daysArray, daysArray.Length - 2, guidArray, guidArray.Length - 6, 2);
            Array.Copy(msecsArray, msecsArray.Length - 4, guidArray, guidArray.Length - 4, 4);

            return new Guid(guidArray);
        }
        
        /// <summary>
        /// 创建自定义值的GUID，例如做测试使用
        /// 00000000-0000-0000-0000-000000{0}{1}
        /// </summary>
        /// <param name="seedCount"></param>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static Queue<Guid> CreateMockGuidQueue(int seedCount, string prefix = "")
        {
            if (prefix == null)
            {
                throw new ArgumentNullException("prefix");
            }

            if (seedCount <= 0 || seedCount > 99999)
            {
                throw new ArgumentException("seedCount必须介于1和99999之间");
            }

            int totalCount = seedCount.ToString().Length + prefix.Length;
            if (totalCount > 6)
            {
                throw new ArgumentException("seedCount和prefix必须组成Guid的后六位");
            }

            string zeroFormat = "";
            for (int i = 0; i < 6 - prefix.Length; i++)
            {
                zeroFormat = zeroFormat + "0";
            }

            var guids = new Queue<Guid>();
            for (int i = 1; i <= seedCount; i++)
            {
                guids.Enqueue(new Guid(string.Format("00000000-0000-0000-0000-000000{0}{1}", prefix, i.ToString(zeroFormat))));
            }

            return guids;
        }
        
        /// <summary>
        /// 创建自定义值的GUID，例如做测试使用
        /// {0}-0000-0000-0000-{1}
        /// </summary>
        /// <param name="seedCount"></param>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static Queue<Guid> CreateMockGuidQueue2(long seedCount, string prefix = "")
        {
            if (prefix == null)
            {
                throw new ArgumentNullException("prefix");
            }

            if (seedCount <= 0 || seedCount > 999999999999)
            {
                throw new ArgumentException("seedCount必须介于1和999999999999之间");
            }

            if (prefix.Length > 8)
            {
                throw new ArgumentException("prefix长度不能超过8");
            }
            

            //pre append
            string prefixAppend = prefix;
            for (int i = 0; i < 8 - prefix.Length; i++)
            {
                prefixAppend = prefixAppend + "0";
            }

            //last append
            string zeroFormat = "";
            for (int i = 0; i < 12; i++)
            {
                zeroFormat = zeroFormat + "0";
            }


            var guids = new Queue<Guid>();
            for (int i = 1; i <= seedCount; i++)
            {
                //XXXXXXXX-0000-0000-0000-00000000000?
                guids.Enqueue(new Guid(string.Format("{0}-0000-0000-0000-{1}", prefixAppend, i.ToString(zeroFormat))));
            }
            return guids;
        }
        
        /// <summary>
        /// 生成10进制的占位格式符
        /// 2=>00, 3=>000
        /// </summary>
        /// <param name="placeHolderCount"></param>
        /// <returns></returns>
        public static string ToNumberFormat(int placeHolderCount)
        {
            if (placeHolderCount <= 0)
            {
                throw new ArgumentException("count should big then 0", "placeHolderCount");
            }
            return string.Join("", Enumerable.Repeat('0', placeHolderCount));
        }
    }
}