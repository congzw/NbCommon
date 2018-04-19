using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NbPilot.Common.Supports
{
    [TestClass]
    public class FeatureSupportTableSpec
    {
        [TestMethod]
        public void Init_Null_Should_ThrowEx()
        {
            var featureSupportTable = new FeatureSupportTable();
            AssertHelper.ShouldThrows<ArgumentNullException>(() =>
            {
                featureSupportTable.Init(null, new List<Product>(), new List<FeatureSupport>());
            });
            AssertHelper.ShouldThrows<ArgumentNullException>(() =>
            {
                featureSupportTable.Init(new List<Feature>(), null, new List<FeatureSupport>());
            });
            AssertHelper.ShouldThrows<ArgumentNullException>(() =>
            {
                featureSupportTable.Init(new List<Feature>(), new List<Product>(), null);
            });
        }
        
        [TestMethod]
        public void Init_RepeatFeatureOrProduct_Should_ThrowEx()
        {
            var featureSupportTable = new FeatureSupportTable();

            AssertHelper.ShouldThrows<InvalidOperationException>(() =>
            {
                var features = new List<Feature>();
                features.Add(new Feature() { Code = "FeatureA", SinceVersion = "1.0.0", DeadVersion = null });
                features.Add(new Feature() { Code = "FeatureA", SinceVersion = "2.0.0", DeadVersion = null });
                featureSupportTable.Init(features, new List<Product>(), new List<FeatureSupport>());
            });

            AssertHelper.ShouldThrows<InvalidOperationException>(() =>
            {
                var products = new List<Product>();
                products.Add(new Product() { Code = "ProductA" });
                products.Add(new Product() { Code = "ProductA" });
                products.Add(new Product() { Code = "ProductA" });
                featureSupportTable.Init(new List<Feature>(), products, new List<FeatureSupport>());
            });
        }

        [TestMethod]
        public void Init_UnknowFeature_Should_ThrowEx()
        {
            var featureSupportTable = new FeatureSupportTable();

            AssertHelper.ShouldThrows<InvalidOperationException>(() =>
            {
                var features = Mocks.CreateNormalFeatures();

                var products = new List<Product>();
                products.Add(new Product() { Code = "ProductA" });
                products.Add(new Product() { Code = "ProductB" });

                var featureSupports = new List<FeatureSupport>();
                featureSupports.Add(new FeatureSupport("FeatureX", "ProductA"));

                featureSupportTable.Init(features, products, featureSupports);
            });
        }

        [TestMethod]
        public void Init_UnknowProduct_Should_ThrowEx()
        {
            var featureSupportTable = new FeatureSupportTable();

            AssertHelper.ShouldThrows<InvalidOperationException>(() =>
            {
                var features = Mocks.CreateNormalFeatures();

                var products = new List<Product>();
                products.Add(new Product() { Code = "ProductA" });
                products.Add(new Product() { Code = "ProductB" });

                var featureSupports = new List<FeatureSupport>();
                featureSupports.Add(new FeatureSupport("FeatureA", "ProductX"));

                featureSupportTable.Init(features, products, featureSupports);
            });
        }

        [TestMethod]
        public void Init_RepeatFeatureSupport_Should_OK()
        {
            var featureSupportTable = new FeatureSupportTable();
            var features = Mocks.CreateNormalFeatures();

            var products = new List<Product>();
            products.Add(new Product() { Code = "ProductA" });
            products.Add(new Product() { Code = "ProductB" });

            var featureSupports = new List<FeatureSupport>();
            featureSupports.Add(new FeatureSupport("FeatureA", "ProductA"));
            featureSupports.Add(new FeatureSupport("FeatureA", "ProductB"));
            featureSupports.Add(new FeatureSupport("FeatureB", "ProductB"));
            featureSupports.Add(new FeatureSupport("FeatureB", "ProductB"));

            featureSupportTable.Init(features, products, featureSupports);

            featureSupportTable.LogProperties();
            featureSupportTable.FeatureSupports.Count.ShouldEqual(3);
        }


        [TestMethod]
        public void IsFeatureSupport_Should_OK()
        {
            var featureSupportTable = new FeatureSupportTable();

            var features = Mocks.CreateNormalFeatures();

            var products = new List<Product>();
            products.Add(new Product() { Code = "ProductA" });
            products.Add(new Product() { Code = "ProductB" });

            var featureSupports = new List<FeatureSupport>();
            featureSupports.Add(new FeatureSupport("FeatureA", "ProductA"));
            featureSupports.Add(new FeatureSupport("FeatureA", "ProductB"));
            featureSupports.Add(new FeatureSupport("FeatureB", "ProductB"));

            featureSupportTable.Init(features, products, featureSupports);
            //todo before version
            featureSupportTable.IsFeatureSupport("FeatureA", "ProductA", "1.0.0").ShouldTrue();
            featureSupportTable.IsFeatureSupport("FeatureA", "ProductA", "1.0.1").ShouldTrue();

            featureSupportTable.IsFeatureSupport("FeatureB", "ProductA", "2.0.0").ShouldFalse();
            featureSupportTable.IsFeatureSupport("FeatureB", "ProductA", "2.0.0").ShouldFalse();

            featureSupportTable.IsFeatureSupport("FeatureC", "ProductA", "3.0.0").ShouldFalse();
            featureSupportTable.IsFeatureSupport("FeatureC", "ProductB", "3.0.0").ShouldFalse();
        }
    }
}
