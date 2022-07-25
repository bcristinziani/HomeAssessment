using Microsoft.VisualStudio.TestTools.UnitTesting;
using HomeAsessment.Business;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace HomeAsessment.Business.Tests
{
    [TestClass()]
    public class VersionManagementTests
    {
        [TestMethod()]
        public void VersionManagement_NoArgs_Passed_In_Should_Return_Error_Message()
        {
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);

                var vm = new VersionManagement();
                vm.Start(new string[] { });

                string expected = "Something went wrong with the program. Check logs for additional details.";

                Assert.AreEqual(expected, sw.ToString().Trim());
            }
        }

        [TestMethod()]
        public void VersionManagement_WrongArgs_Passed_In_Should_Return_Error_Message()
        {
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);

                var vm = new VersionManagement();
                vm.Start(new string[] { "Wrong Value" });

                string expected = "The value passed in, is not recognizable. Accepted values are \"Feature\" or \"Bug Fix\".";

                Assert.AreEqual(expected, sw.ToString().Trim());
            }
        }

        [TestMethod()]
        public void VersionManagement_MajorVersion_Returned()
        {
            DeleteProductFile();

            string expectedVersion = "1.0.1.0";
            var vm = new VersionManagement();

            string version = vm.IncrementMajorVersion();

            Assert.AreEqual(expectedVersion, version);
        }

        [TestMethod()]
        public void VersionManagement_MinorVersion_Returned()
        {
            DeleteProductFile();

            string expectedVersion = "1.0.0.1";
            var vm = new VersionManagement();

            string version = vm.IncrementMinorVersion();

            Assert.AreEqual(expectedVersion, version);
        }

        [TestMethod()]
        public void VersionManagement_MajorVersion_Return_With_Zero_Minor_Release()
        {
            DeleteProductFile();

            string expectedVersion = "1.0.2.0";
            var vm = new VersionManagement();

            vm.IncrementMajorVersion();
            vm.IncrementMinorVersion();

            string majorVersion = vm.IncrementMajorVersion();

            Assert.AreEqual(expectedVersion, majorVersion);
        }

        private void DeleteProductFile()
        {
            // Delete ProductInfo.cs file if it exists so that we have consistency with testing
            var file = $@"Version\ProductInfo.cs";
            if (File.Exists(file))
            {
                File.Delete(file);
            }
        }
    }
}