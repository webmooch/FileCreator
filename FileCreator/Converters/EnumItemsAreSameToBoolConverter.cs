using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace FileCreator.Converters
{
    [ValueConversion(typeof(Enum), typeof(bool), ParameterType = typeof(Enum))]
    internal class EnumItemsAreSameToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || !(value is Enum))
                return DependencyProperty.UnsetValue;

            if (parameter == null || !(parameter is Enum))
                return DependencyProperty.UnsetValue;

            return value.Equals(parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.Equals(true) ? parameter : DependencyProperty.UnsetValue;
        }
    }
}
