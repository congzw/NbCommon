using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NbPilot.Common.AppData
{
    [TestClass]
    public class FileDbHelperSpec
    {
        [TestMethod]
        public void Read_NotExist_Should_ReturnEmpty()
        {
            var fileDbHelper = new FileDbHelper();
            var whaters = fileDbHelper.Read<Object>("NotExist");
            whaters.ShouldNotNull();
            whaters.Count.ShouldEqual(0);
        }
    }
}
