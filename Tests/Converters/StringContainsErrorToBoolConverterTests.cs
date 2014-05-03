using FileCreator.Converters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows;

namespace Tests.Converters
{
    [TestClass]
    public class StringContainsErrorToBoolConverterTests
    {
        private static StringContainsErrorToBoolConverter converter = new StringContainsErrorToBoolConverter();

        [TestMethod]
        public void StringContainsErrorToBoolConverter_ContainsTest()
        {
            Assert.IsTrue((bool)converter.Convert("This string contains the word error.", null, null, null));
        }

        [TestMethod]
        public void StringContainsErrorToBoolConverter_StartsWithTest()
        {
            Assert.IsTrue((bool)converter.Convert("Error is the first word of this string.", null, null, null));
        }

        [TestMethod]
        public void StringContainsErrorToBoolConverter_FalseTest()
        {
            Assert.IsFalse((bool)converter.Convert("This string does not contain the 'e' word.", null, null, null));
        }

        [TestMethod]
        public void StringContainsErrorToBoolConverter_EmptyTest()
        {
            Assert.IsFalse((bool)converter.Convert(string.Empty, null, null, null));
        }

        [TestMethod]
        public void StringContainsErrorToBoolConverter_NullTest()
        {
            Assert.AreSame(DependencyProperty.UnsetValue, converter.Convert(null, null, null, null));
        }

        [TestMethod]
        public void StringContainsErrorToBoolConverter_JunkTest()
        {
            Assert.AreSame(DependencyProperty.UnsetValue, converter.Convert(new object(), null, null, null));
        }
    }
}
