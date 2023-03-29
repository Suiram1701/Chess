using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace Chess.Figures.Convert
{
    internal class DataToImageConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Get data
            Color Color = (Color)value;
            string TargetFigure = (string)parameter;

            // Return image source
            BitmapImage BmI = new BitmapImage();
            BmI.BeginInit();
            BmI.UriSource = new Uri($"/Chess.Figures;component/Resources/{TargetFigure}{Color}.png", UriKind.Relative);
            BmI.EndInit();
            return BmI;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
