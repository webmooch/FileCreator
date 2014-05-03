using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Data;

namespace FileCreator.Converters
{
    [ValueConversion(typeof(string), typeof(bool))]
    internal class StringContainsErrorToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var message = value as string;
            return (message != null) ? Regex.IsMatch(message, @"(\s|^)error", RegexOptions.IgnoreCase) : DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
