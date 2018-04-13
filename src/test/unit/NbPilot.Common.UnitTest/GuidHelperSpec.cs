using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NbPilot.Common
{
    [TestClass]
    public class GuidHelperSpec
    {
        [TestMethod]
        public void CreateMockGuidQueue_WithoutPrefix_Should_OK()
        {
            var guids = GuidHelper.CreateMockGuidQueue(3).ToList();
            guids.Count.ShouldEqual(3);
            guids[0].ShouldEqual(new Guid("00000000-0000-0000-0000-000000000001"));
            guids[1].ShouldEqual(new Guid("00000000-0000-0000-0000-000000000002"));
            guids[2].ShouldEqual(new Guid("00000000-0000-0000-0000-000000000003"));
        }
        [TestMethod]
        public void CreateMockGuidQueue_WithPrefix_Should_OK()
        {
            var guids = GuidHelper.CreateMockGuidQueue(3, "abc").ToList();
            guids.Count.ShouldEqual(3);
            guids[0].ShouldEqual(new Guid("00000000-0000-0000-0000-000000abc001"));
            guids[1].ShouldEqual(new Guid("00000000-0000-0000-0000-000000abc002"));
            guids[2].ShouldEqual(new Guid("00000000-0000-0000-0000-000000abc003"));
        }

        [TestMethod]
        public void CreateMockGuidQueue2_WithoutPrefix_Should_OK()
        {
            var guids = GuidHelper.CreateMockGuidQueue(3).ToList();
            guids.Count.ShouldEqual(3);
            guids[0].ShouldEqual(new Guid("00000000-0000-0000-0000-000000000001"));
            guids[1].ShouldEqual(new Guid("00000000-0000-0000-0000-000000000002"));
            guids[2].ShouldEqual(new Guid("00000000-0000-0000-0000-000000000003"));
        }
        [TestMethod]
        public void CreateMockGuidQueue2_WithPrefix_Should_OK()
        {
            var guids = GuidHelper.CreateMockGuidQueue2(3,  "abc").ToList();
            guids.Count.ShouldEqual(3);
            guids[0].ShouldEqual(new Guid("abc00000-0000-0000-0000-000000000001"));
            guids[1].ShouldEqual(new Guid("abc00000-0000-0000-0000-000000000002"));
            guids[2].ShouldEqual(new Guid("abc00000-0000-0000-0000-000000000003"));
        }
        
        [TestMethod]
        public void GenerateComb_MutliTime_Should_Different()
        {
            var guid = GuidHelper.GenerateComb();
            var guid2 = GuidHelper.GenerateComb();
            guid.ShouldNotEqual(guid2);
        }
        
        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void ToNumberFormat_Input0_Should_ThrowEx()
        {
            GuidHelper.ToNumberFormat(0);
        }
        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void ToNumberFormat_InputLessThen0_Should_ThrowEx()
        {
            GuidHelper.ToNumberFormat(-1);
        }
        [TestMethod]
        public void ToNumberFormat_InputMoreThen0_Should_OK()
        {
            GuidHelper.ToNumberFormat(1).ShouldEqual("0");
            GuidHelper.ToNumberFormat(3).ShouldEqual("000");
            GuidHelper.ToNumberFormat(5).ShouldEqual("00000");
        }
    }
}
