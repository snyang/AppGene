using AppGene.Common.Entities;
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
    public class EnumToBooleanConverterTest
    {
        [Test]
        [TestCase(Gender.Female, true)]
        [TestCase(Gender.Female, false)]
        [TestCase(Gender.Male, true)]
        [TestCase(Gender.Male, false)]
        public void ConvertTest(Gender value, bool expectedResult)
        {
            string argument = expectedResult ? value.ToString() : "";
            var converter = new EnumToBooleanConverter();
            bool converted = (bool)converter.Convert(value, typeof(bool), argument, CultureInfo.CurrentCulture);
            Assert.AreEqual(expectedResult, converted);
        }

        [Test]
        [TestCase(Gender.Female, "Female", true)]
        [TestCase(null, "Female", false)]
        [TestCase(Gender.Male, "Male", true)]
        [TestCase(null, "Male", false)]
        public void ConvertBackTest(object expectedValue, string argument, bool value)
        {
            var converter = new EnumToBooleanConverter();
            object returnValue = converter.ConvertBack(value, typeof(Gender), argument, CultureInfo.CurrentCulture);
            Assert.AreEqual(expectedValue, returnValue);
        }
    }
}
