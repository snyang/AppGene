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
    public class Int32ToDateConverterTest
    {
        [Test]
        [TestCase(20010131, "2001-01-31")]
        [TestCase(99991231, "9999-12-31")]
        [TestCase(99991331, "2000-01-01")]
        public void ConvertTest(object value, string result)
        {
            var converter = new Int32ToDateConverter();
            DateTime date = (DateTime)converter.Convert(value, typeof(DateTime), null, CultureInfo.CurrentCulture);
            Assert.AreEqual(DateTime.ParseExact(result, "yyyy-MM-dd", CultureInfo.InvariantCulture), date);
        }

        [Test]
        [TestCase(20010131, "2001-01-31")]
        [TestCase(99991231, "9999-12-31")]
        [TestCase(20000101, "2000-01-01")]
        public void ConvertBackTest(object value, string dateString)
        {
            var converter = new Int32ToDateConverter();
            Int32 returnValue = (Int32)converter.ConvertBack(DateTime.ParseExact(dateString, "yyyy-MM-dd", CultureInfo.InvariantCulture),
                typeof(Int32), null, CultureInfo.CurrentCulture);
            Assert.AreEqual(value, returnValue);
        }
    }
}
