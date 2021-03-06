﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NbPilot.Common
{
    [TestClass]
    public class MyAppPathSpec
    {
        [TestMethod]
        public void IsWebApp_Should_False()
        {
            var appFolderPath = new MyAppPath();
            appFolderPath.LogProperties();
            appFolderPath.IsWebApp.ShouldFalse();
        }

        [TestMethod]
        public void BaseDirectory_Should_NotNull()
        {
            var appFolderPath = new MyAppPath();
            appFolderPath.LogProperties();
            appFolderPath.BaseDirectory.ShouldNotNull();
        }
        
        [TestMethod]
        public void AppData_Should_NotNull()
        {
            var appFolderPath = new MyAppPath();
            appFolderPath.LogProperties();
            appFolderPath.AppData.ShouldNotNull();
        }

        [TestMethod]
        public void Bin_Should_NotNull()
        {
            var appFolderPath = new MyAppPath();
            appFolderPath.LogProperties();
            appFolderPath.Bin.ShouldNotNull();
        }

        [TestMethod]
        public void CreateSubFolder_Should_OK()
        {
            var appFolderPath = new MyAppPath();
            appFolderPath.LogProperties();
            appFolderPath.CombinePath("A", "B").ShouldEqual(@"A\B");
            appFolderPath.CombinePath(@"A\", "B").ShouldEqual(@"A\B");
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void CreateSubFolder_BasePath_Null_Should_Throw()
        {
            var appFolderPath = new MyAppPath();
            appFolderPath.LogProperties();
            appFolderPath.CombinePath(null, "B");
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void CreateSubFolder_SubPath_Null_Should_Throw()
        {
            var appFolderPath = new MyAppPath();
            appFolderPath.LogProperties();
            appFolderPath.CombinePath("A", null);
        }
    }
}
