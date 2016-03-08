using System;
using System.Globalization;
using System.Windows.Data;

namespace AppGene.Ui.Infrastructure.Converters
{
    /// <summary>
    /// Converts string value to date.
    /// The format of the string is YYYYMMDD
    /// </summary>
    public class StringToDateConverter : IValueConverter
    {
        private DateTime defaultDate = new DateTime(2000, 1, 1);
        private string format = "yyyyMMdd";
        private string Format
        {
            get
            {
                return format;
            }
            set
            {
                if (string.IsNullOrEmpty(value)) return;
                this.format = value;
            }
        } 

        public StringToDateConverter()
        { }

        public StringToDateConverter(params object[] args)
        {
            if (args == null) return;
            if (args.Length == 1)
            {
                this.Format = (string)args[0];
                return;
            }
            if (args.Length == 2)
            {
                this.Format = (string)args[0];
                this.defaultDate = (DateTime)args[1];
                return;
            }
        }

        public StringToDateConverter(string format)
        {
            this.Format = format;
        }

        public StringToDateConverter(string format, DateTime defaultDate)
            : this(format)
        {
            this.defaultDate = defaultDate;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime returnValue = defaultDate;

            if (value == null) return returnValue;

            string stringValue = (string)value;
            if (stringValue.Length == 0) return returnValue;

            if (!DateTime.TryParseExact(stringValue, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out returnValue))
            {
                returnValue = defaultDate;
            }

            return returnValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string returnValue = "";

            if (value == null) return returnValue;

            DateTime dateValue = (DateTime)value;

            returnValue = dateValue.ToString(format, CultureInfo.InvariantCulture);

            return returnValue;
        }
    }
}
