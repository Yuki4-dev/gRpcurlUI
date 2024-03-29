﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace gRpcurlUI.Converters
{
    public class BooleanBackConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            
            if(value is bool bValue)
            {
                return !bValue;
            }
            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
