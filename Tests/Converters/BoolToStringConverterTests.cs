using FileCreator.Converters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Windows;

namespace Tests.Converters
{
    [TestClass]
    public class BoolToStringConverterTests
    {
        private static string trueValue = "True value";
        private static string falseValue = "False value";
        private static BoolToStringConverter converter = new BoolToStringConverter() { TrueValue = trueValue, FalseValue = falseValue };

        [TestMethod]
        public void BoolToStringConverter_TrueTest()
        {
            Assert.IsTrue(trueValue == (string)converter.Convert(true, null, null, null));
        }

        [TestMethod]
        public void BoolToStringConverter_FalseTest()
        {
            Assert.IsTrue(falseValue == (string)converter.Convert(false, null, null, null));
        }

        [TestMethod]
        public void BoolToStringConverter_NullTest()
        {
            Assert.IsTrue(DependencyProperty.UnsetValue == converter.Convert(null, null, null, null));
        }

        [TestMethod]
        public void BoolToStringConverter_JunkTest()
        {
            Assert.IsTrue(DependencyProperty.UnsetValue == converter.Convert(DayOfWeek.Friday, null, null, null));
        }
    }
}
