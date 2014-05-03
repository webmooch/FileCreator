﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace FileCreator.Converters
{
    [ValueConversion(typeof(bool), typeof(bool))]
    internal class BoolToInverseBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool))
                return DependencyProperty.UnsetValue;

            return !(bool)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
