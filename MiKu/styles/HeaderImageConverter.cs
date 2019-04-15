using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace MiKu
{
    public class HeaderImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                var tag = value.ToString();
                return new BitmapImage(new Uri(Environment.CurrentDirectory + $"\\images\\header\\header_{tag}.png", UriKind.RelativeOrAbsolute));
            }
            catch (Exception)
            {
                return new BitmapImage(new Uri(Environment.CurrentDirectory + $"\\images\\header\\header_default.png", UriKind.RelativeOrAbsolute));
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}