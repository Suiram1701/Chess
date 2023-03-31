using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Chess.App.Convert
{
    [ValueConversion(typeof(Point), typeof(string))]
    internal class PointStringConvert : IValueConverter
    {
        private readonly char[] Letters = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H' };

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Point P = (Point)value;
            return Letters[(int)P.X].ToString() + (P.Y + 1);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
