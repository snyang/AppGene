using System;
using System.Globalization;
using System.Windows.Data;

namespace AppGene.Ui.Infrastructure
{
    //http://wpftutorial.net/RadioButton.html
    public class EnumToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null) return false;

            string checkValue = value.ToString();
            string targetValue = parameter.ToString();
            return checkValue.Equals(targetValue, StringComparison.InvariantCultureIgnoreCase);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null) return null;

            bool useValue = (bool)value;
            string targetValue = parameter.ToString();
            if (useValue) return Enum.Parse(targetType, targetValue);

            return null;
        }
    }
}