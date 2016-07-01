using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;

namespace ISafe_UserClient
{
    public class StringToImageCvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                try
                {
                    var path = value.ToString();
                   
                    return new System.Windows.Media.Imaging.BitmapImage(new Uri(path, UriKind.RelativeOrAbsolute));
                }
                catch
                {
                    return null;
                }
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }

    public class StringToBrushCvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                try
                {
                    var path = value.ToString();
                    if (path != null)
                    {
                        if (path.ToLower().Equals("true"))
                        {
                            return new SolidColorBrush(Colors.Green);
                        }
                        else if (path.ToLower().Equals("false"))
                        {
                            return new SolidColorBrush(Colors.Red);
                        }
                    }
                    else 
                    {
                        return null;
                    }
                }
                catch
                {
                    return null;
                }
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
   
}
