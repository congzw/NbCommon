using System;
using System.Collections.Generic;
using System.Linq;

namespace NbPilot.Common.FeatureSupports
{
    public class FeatureSupportTable
    {
        public IReadOnlyList<Feature> Features { get; private set; }
        public IReadOnlyList<Product> Products { get; private set; }
        public IReadOnlyList<FeatureSupport> FeatureSupports { get; private set; }

        public FeatureSupportTable()
        {
            Products = new List<Product>();
            Features = new List<Feature>();
            FeatureSupports = new List<FeatureSupport>();
        }

        public void Init(IList<Feature> features, IList<Product> products, IList<FeatureSupport> featureSupports)
        {
            if (features == null)
            {
                throw new ArgumentNullException("features");
            }
            if (products == null)
            {
                throw new ArgumentNullException("products");
            }
            if (featureSupports == null)
            {
                throw new ArgumentNullException("featureSupports");
            }

            var featureGroups = features.GroupBy(x => x.Code).ToList();
            foreach (var featureGroup in featureGroups)
            {
                var count = featureGroup.Count();
                if (count > 1)
                {
                    throw new InvalidOperationException(string.Format("发现重复的Feature注册项{0}，共计: {1}", featureGroup.Key, count));
                }
            }
            var productGroups = products.GroupBy(x => x.Code).ToList();
            foreach (var productGroup in productGroups)
            {
                var count = productGroup.Count();
                if (count > 1)
                {
                    throw new InvalidOperationException(string.Format("发现重复的Product注册项{0}，共计: {1}", productGroup.Key, count));
                }
            }

            var featureSupportsFix = new List<FeatureSupport>();
            foreach (var featureSupport in featureSupports)
            {
                var feature = features.SingleOrDefault(x => x.Code == featureSupport.FeatureCode);
                if (feature == null)
                {
                    throw new InvalidOperationException(string.Format("非法的功能注册项{0}-{1}，因为Feature {0} 未登记", featureSupport.FeatureCode, featureSupport.ProductCode));
                }
                var product = products.SingleOrDefault(x => x.Code == featureSupport.ProductCode);
                if (product == null)
                {
                    throw new InvalidOperationException(string.Format("非法的功能注册项{0}-{1}，因为Product {1} 未登记", featureSupport.FeatureCode, featureSupport.ProductCode));
                }

                var theOne = featureSupportsFix.SingleOrDefault(
                    x => x.FeatureCode == featureSupport.FeatureCode && x.ProductCode == featureSupport.ProductCode);
                if (theOne == null)
                {
                    //防止重复注册
                    featureSupportsFix.Add(featureSupport);
                }
            }

            Features = features.ToList();
            Products = products.ToList();
            FeatureSupports = featureSupportsFix.ToList();
        }

        public bool IsFeatureSupport(string featureCode, string productCode, string currentVersion)
        {
            var version = new Version(currentVersion);

            var feature = FeatureSupports.SingleOrDefault(x => x.ProductCode == productCode && x.FeatureCode == featureCode);
            if (feature == null)
            {
                return false;
            }

            var theFeature = Features.SingleOrDefault(x => x.Code == featureCode);
            if (theFeature == null)
            {
                return false;
            }

            var theProduct = Products.SingleOrDefault(x => x.Code == productCode);
            if (theProduct == null)
            {
                return false;
            }

            return version >= new Version(theFeature.SinceVersion);
        }
    }

    #region dtos

    public class Feature
    {
        /// <summary>
        /// 功能代码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 从此版本开始支持
        /// </summary>
        public string SinceVersion { get; set; }
        /// <summary>
        /// 从此版本开始不支持
        /// </summary>
        public string DeadVersion { get; set; }
    }
    public class Product
    {
        public string Code { get; set; }
    }
    public class FeatureSupport
    {
        public FeatureSupport(string featureCode, string productCode)
        {
            if (string.IsNullOrWhiteSpace(featureCode))
            {
                throw new ArgumentNullException("featureCode");
            }
            if (string.IsNullOrWhiteSpace(productCode))
            {
                throw new ArgumentNullException("productCode");
            }

            FeatureCode = featureCode;
            ProductCode = productCode;
        }

        public string FeatureCode { get; set; }
        public string ProductCode { get; set; }
    }

    #endregion
}
