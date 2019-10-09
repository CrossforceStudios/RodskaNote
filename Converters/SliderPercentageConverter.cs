using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;

namespace RodskaNote.Converters
{
    public class SliderPercentageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int percent = (int)(((double)value) * 100);
            return percent.ToString() + "%";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string pcS = value.ToString();
            int percent = int.Parse(pcS.Substring(0, pcS.Length - 1));
            double val = (percent / 100);
            return val;
        }
    }

    public class SliderPercentageExtension: MarkupExtension
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return new SliderPercentageConverter();
        }
    }
}
