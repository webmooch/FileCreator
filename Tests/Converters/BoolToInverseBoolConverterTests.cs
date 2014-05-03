using FileCreator.Converters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Windows;

namespace Tests.Converters
{
    [TestClass]
    public class BoolToInverseBoolConverterTests
    {
        private static BoolToInverseBoolConverter converter = new BoolToInverseBoolConverter();

        [TestMethod]
        public void BoolToInverseBoolConverter_TrueTest()
        {
            Assert.IsFalse((bool)converter.Convert(true, null, null, null));
        }

        [TestMethod]
        public void BoolToInverseBoolConverter_FalseTest()
        {
            Assert.IsTrue((bool)converter.Convert(false, null, null, null));
        }

        [TestMethod]
        public void BoolToInverseBoolConverter_NullTest()
        {
            Assert.IsTrue(DependencyProperty.UnsetValue == converter.Convert(null, null, null, null));
        }

        [TestMethod]
        public void BoolToInverseBoolConverter_JunkTest()
        {
            Assert.IsTrue(DependencyProperty.UnsetValue == converter.Convert(DayOfWeek.Friday, null, null, null));
        }
    }
}
