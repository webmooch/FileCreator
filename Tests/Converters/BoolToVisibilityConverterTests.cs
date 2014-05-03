using FileCreator.Converters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Windows;

namespace Tests.Converters
{
    [TestClass]
    public class BoolToVisibilityConverterTests
    {
        private static Visibility trueValue = Visibility.Visible;
        private static Visibility falseValue = Visibility.Hidden;
        private static BoolToVisibilityConverter converter = new BoolToVisibilityConverter() { TrueValue = trueValue, FalseValue = falseValue };

        [TestMethod]
        public void BoolToVisibilityConverter_TrueTest()
        {
            Assert.IsTrue(trueValue == (Visibility)converter.Convert(true, null, null, null));
        }

        [TestMethod]
        public void BoolToVisibilityConverter_FalseTest()
        {
            Assert.IsTrue(falseValue == (Visibility)converter.Convert(false, null, null, null));
        }

        [TestMethod]
        public void BoolToVisibilityConverter_NullTest()
        {
            Assert.IsTrue(DependencyProperty.UnsetValue == converter.Convert(null, null, null, null));
        }

        [TestMethod]
        public void BoolToVisibilityConverter_JunkTest()
        {
            Assert.IsTrue(DependencyProperty.UnsetValue == converter.Convert(DayOfWeek.Friday, null, null, null));
        }
    }
}
