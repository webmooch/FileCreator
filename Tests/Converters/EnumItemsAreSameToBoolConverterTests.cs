using FileCreator.Converters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Windows;

namespace Tests.Converters
{
    [TestClass]
    public class EnumItemsAreSameToBoolConverterTests
    {
        private static EnumItemsAreSameToBoolConverter converter = new EnumItemsAreSameToBoolConverter();

        [TestMethod]
        public void EnumItemsAreSameToBoolConverter_TrueTest()
        {
            Assert.IsTrue((bool)converter.Convert(DayOfWeek.Friday, null, DayOfWeek.Friday, null));
        }

        [TestMethod]
        public void EnumItemsAreSameToBoolConverter_FalseTest()
        {
            Assert.IsFalse((bool)converter.Convert(DayOfWeek.Friday, null, DayOfWeek.Monday/*not a chance*/, null));
        }

        [TestMethod]
        public void EnumItemsAreSameToBoolConverter_NullValueTest()
        {
            Assert.IsTrue(DependencyProperty.UnsetValue == converter.Convert(null, null, DayOfWeek.Friday, null));
        }

        [TestMethod]
        public void EnumItemsAreSameToBoolConverter_NullParamTest()
        {
            Assert.IsTrue(DependencyProperty.UnsetValue == converter.Convert(DayOfWeek.Friday, null, null, null));
        }

        [TestMethod]
        public void EnumItemsAreSameToBoolConverter_NullTest()
        {
            Assert.IsTrue(DependencyProperty.UnsetValue == converter.Convert(null, null, null, null));
        }

        [TestMethod]
        public void EnumItemsAreSameToBoolConverter_JunkValueTest()
        {
            Assert.IsTrue(DependencyProperty.UnsetValue == converter.Convert(new object(), null, DayOfWeek.Friday, null));
        }

        [TestMethod]
        public void EnumItemsAreSameToBoolConverter_JunkParamTest()
        {
            Assert.IsTrue(DependencyProperty.UnsetValue == converter.Convert(DayOfWeek.Friday, null, new object(), null));
        }
    }
}
