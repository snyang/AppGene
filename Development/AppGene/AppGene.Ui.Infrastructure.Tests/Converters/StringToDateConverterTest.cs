using AppGene.Ui.Infrastructure.Converters;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppGene.Ui.Infrastructure.Tests.Converters
{
    [TestFixture]
    public class StringToDateConverterTest
    {
        [Test]
        [TestCase("20010131", "yyyyMMdd", "2001-01-31")]
        [TestCase("9999-12-31", "yyyy-MM-dd", "9999-12-31")]
        [TestCase("99991331", "yyyyMMdd", "2000-01-01")]
        public void ConvertTest(string value, string format, string expectedResult)
        {
            var converter = new StringToDateConverter(format);
            DateTime date = (DateTime)converter.Convert(value, typeof(DateTime), null, CultureInfo.CurrentCulture);
            Assert.AreEqual(DateTime.ParseExact(expectedResult, "yyyy-MM-dd", CultureInfo.InvariantCulture), date);
        }

        [Test]
        [TestCase("20010131", "yyyyMMdd", "2001-01-31")]
        [TestCase("9999-12-31", "yyyy-MM-dd", "9999-12-31")]
        [TestCase("20000101", "yyyyMMdd", "2000-01-01")]
        public void ConvertBackTest(string expecteValue, string format, string dateString)
        {
            var converter = new StringToDateConverter(format);
            string returnValue = (string)converter.ConvertBack(DateTime.ParseExact(dateString, "yyyy-MM-dd", CultureInfo.InvariantCulture),
                typeof(Int32), null, CultureInfo.CurrentCulture);
            Assert.AreEqual(expecteValue, returnValue);
        }
    }
}
