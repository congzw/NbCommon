using System;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

// ReSharper disable CheckNamespace

namespace NbPilot.Common
{
    /// <summary>
    /// 对象Hash帮助类
    /// </summary>
    public interface IObjectHashHelper
    {
        /// <summary>
        /// 获取对象Hash值
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        string CreateObjectHash(object obj);

        /// <summary>
        /// 校验对象Hash值
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="hash"></param>
        /// <returns></returns>
        bool VerifyObjectHash(object obj, string hash);
    }

    /// <summary>
    /// 对象Hash帮助类
    /// </summary>
    public class ObjectHashHelper : IObjectHashHelper
    {
        public string CreateObjectHash(object obj)
        {
            if (obj == null)
            {
                return string.Empty;
            }
            var computeHash = EasyMD5.HashObject(obj);
            return computeHash;
        }

        public bool VerifyObjectHash(object obj, string hash)
        {
            var computeHash = CreateObjectHash(obj);
            return 0 == StringComparer.OrdinalIgnoreCase.Compare(computeHash, hash);
        }

        #region for di extensions

        private static Func<IObjectHashHelper> _resolve = () => ResolveAsSingleton.Resolve<ObjectHashHelper, IObjectHashHelper>();
        public static Func<IObjectHashHelper> Resolve
        {
            get { return _resolve; }
            set { _resolve = value; }
        }

        #endregion

        internal class EasyMD5
        {
            public static string HashBytes(byte[] bytes)
            {
                if (bytes == null)
                {
                    return string.Empty;
                }
                using (var md5 = MD5.Create())
                {
                    return ConvertHashBytesToString(md5.ComputeHash(bytes));
                }
            }

            public static string HashString(string str)
            {
                if (str == null)
                {
                    return string.Empty;
                }
                var bytes = Encoding.UTF8.GetBytes(str);
                return HashBytes(bytes);
            }

            public static string HashObject(object obj)
            {
                if (obj == null)
                {
                    return string.Empty;
                }

                var str = JsonConvert.SerializeObject(obj);
                return HashString(str);
            }

            //helpers
            private static string ConvertHashBytesToString(byte[] bytes)
            {
                var sBuilder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    sBuilder.Append(bytes[i].ToString("x2"));
                }
                return sBuilder.ToString();
            }
        }
    }

    #region extensions

    /// <summary>
    /// 计算对象Hash的扩展
    /// </summary>
    public static class ObjectHashExtensions
    {
        /// <summary>
        /// 获取对象Hash值
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string CreateObjectHash(this object obj)
        {
            var objectHashHelper = ObjectHashHelper.Resolve();
            return objectHashHelper.CreateObjectHash(obj);
        }

        /// <summary>
        /// 校验对象Hash值
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="hash"></param>
        /// <returns></returns>
        public static bool VerifyObjectHash(this object obj, string hash)
        {
            var objectHashHelper = ObjectHashHelper.Resolve();
            return objectHashHelper.VerifyObjectHash(obj, hash);
        }
    }

    #endregion
}
