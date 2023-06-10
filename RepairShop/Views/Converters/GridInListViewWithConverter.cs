using System;
using System.Globalization;
using System.Windows.Data;

namespace RepairShop.Views.Converters;

public class GridInListViewWithConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (double)value - 20;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (double)value + 20;
    }
}
