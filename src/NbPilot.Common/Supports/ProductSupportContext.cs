using System;
using System.Linq;

namespace NbPilot.Common.Supports
{
    /// <summary>
    /// 产品支持上下文
    /// </summary>
    public interface IProductSupportContext
    {
        /// <summary>
        /// 当前的产品代码
        /// </summary>
        string CurrentProductCode { get; set; }

        /// <summary>
        /// 是否匹配
        /// </summary>
        /// <param name="productCodes"></param>
        /// <returns></returns>
        bool Match(string productCodes);
    }

    /// <summary>
    /// 产品支持上下文的默认实现
    /// </summary>
    public class ProductSupportContext : IProductSupportContext
    {
        public ProductSupportContext()
        {
            CurrentProductCode = DefaultProductCode;
        }

        public string CurrentProductCode { get; set; }
        public bool Match(string productCodes)
        {
            if (string.IsNullOrWhiteSpace(productCodes) || productCodes == "*")
            {
                return true;
            }

            var tryCodes = productCodes.Split(',').Select(x => x.Trim()).ToList();
            //has "*" will match
            var hasStartOrCurrentProductCode = tryCodes.Any(x =>
                x.Equals("*", StringComparison.OrdinalIgnoreCase)
                || x.Equals(CurrentProductCode, StringComparison.OrdinalIgnoreCase)
                );
            return hasStartOrCurrentProductCode;
        }

        public static string DefaultProductCode = "PuJiao";

        #region for di extensions

        private static Func<IProductSupportContext> _resolve = () => ResolveAsSingleton.Resolve<ProductSupportContext, IProductSupportContext>();
        public static Func<IProductSupportContext> Resolve
        {
            get { return _resolve; }
            set { _resolve = value; }
        }

        #endregion
    }
}
