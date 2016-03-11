using System;
using System.Globalization;
using System.Windows.Data;

namespace AppGene.Ui.Infrastructure.Converters
{
    public class Int32ToDateConverter : IValueConverter
    {
        private DateTime defaultDate = new DateTime(2000, 1, 1);
        private string format = "yyyyMMdd";

        public Int32ToDateConverter()
        { }

        public Int32ToDateConverter(params object[] args)
        {
            if (args == null) return;
            if (args.Length == 1)
            {
                this.format = (string)args[0];
                return;
            }
            if (args.Length == 2)
            {
                this.format = (string)args[0];
                this.defaultDate = (DateTime)args[1];
                return;
            }
        }

        public Int32ToDateConverter(string format)
        {
            this.format = format;
        }

        public Int32ToDateConverter(string format, DateTime defaultDate)
        {
            this.format = format;
            this.defaultDate = defaultDate;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime returnValue = defaultDate;

            if (value == null) return returnValue;

            int intValue = (int)value;
            if (intValue == 0) return returnValue;

            string stringValue = intValue.ToString();

            if (!DateTime.TryParseExact(stringValue, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out returnValue))
            {
                returnValue = defaultDate;
            }

            return returnValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int returnValue = 0;

            if (value == null) return returnValue;

            DateTime dateValue = (DateTime)value;

            returnValue = System.Convert.ToInt32(dateValue.ToString(format, CultureInfo.InvariantCulture));

            return returnValue;
        }
    }
}
