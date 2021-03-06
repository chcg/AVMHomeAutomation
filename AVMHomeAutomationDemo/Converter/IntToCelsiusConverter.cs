﻿using System;
using System.Windows.Data;

namespace AVMHomeAutomationDemo.Converter
{
    [ValueConversion(typeof(int?), typeof(string))]
    public class IntToCelsiusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int? val = (int?)value;
            return val.HasValue ? $"{val.Value / 10.0:N1} °C" : "--.- °C";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
