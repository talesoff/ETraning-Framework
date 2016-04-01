using ETE.ViewModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace GLSample.Converter
{
    public class FPSToColorConverter : IValueConverter
    {
        private static readonly SolidColorBrush defaultBrush = new SolidColorBrush(Colors.Yellow);
        private static readonly SolidColorBrush successBrush = new SolidColorBrush(Colors.Blue);
        private static readonly SolidColorBrush failBrush = new SolidColorBrush(Colors.Red);

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            float fps = 0;
            if (false == float.TryParse(value.ToString(), out fps))
            {
                return defaultBrush;
            }

            float th = ETRViewModel.Instance.FPSThreshold;
            if (fps > th)
            {
                return successBrush;
            }
            else
            {
                return failBrush;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
