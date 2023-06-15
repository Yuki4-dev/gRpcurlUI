using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace gRpcurlUI.Converters
{
    public class Boolean2VisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var falseValue = Visibility.Collapsed;
            if(parameter is Visibility vParame)
            {
                falseValue = vParame;
            }

            if(value is bool bValue)
            {
                return bValue ? Visibility.Visible : falseValue;
            }


            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
