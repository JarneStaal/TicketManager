using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace TicketManager.Converters
{
    public class PartialText : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var text = value.ToString();
            if (text != null)
            {
                return text.Substring(0, Math.Min(text.Length, 70)) + " ...." + Environment.NewLine + Environment.NewLine + "Klik hierop voor meer info";
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
