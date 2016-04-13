using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AppGene.Common.Entities.Infrastructure.Tests
{
    public static class ObjectComparator
    {
        public static void AreEqual<T>(T expected, T actual)
        {
            AreEqual<T>(expected, actual, new List<string>(), null);
        }

        public static void AreEqual<T>(T expected, 
            T actual, 
            IList<string> skippedProperties,
            string message)
        {
            CompareNull(expected, actual);


            var properties = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public);

            StringBuilder expectedPropertyContentBuilder = new StringBuilder();
            StringBuilder actualPropertyContentBuilder = new StringBuilder();
            foreach (var property in properties)
            {
                if (!property.CanRead)
                {
                    continue;
                }
                if (skippedProperties.Contains(property.Name))
                {
                    continue;
                }

                if (!property.PropertyType.Equals(typeof(string))) continue;

                // get expected
                string value = property.GetValue(expected) as string;
                if (!string.IsNullOrEmpty(value))
                {
                    expectedPropertyContentBuilder.AppendFormat("    {0}: {1}", property.Name, value);
                }

                // get actual value
                value = property.GetValue(actual) as string;
                if (!string.IsNullOrEmpty(value))
                { 
                    actualPropertyContentBuilder.AppendFormat("    {0}: {1}", property.Name, value);
                }
            }

            foreach (var property in properties)
            {
                if (!property.CanRead || !property.CanWrite)
                {
                    continue;
                }
                if (skippedProperties.Contains(property.Name))
                {
                    continue;
                }
                var expectedPropertyValue = property.GetValue(expected);
                var actualPropertyValue = property.GetValue(actual);
                Assert.AreEqual(expectedPropertyValue, actualPropertyValue,
                    string.Format("{0} Property: {1} Expected Object : {2} Actual Object : {3}", 
                    message, property.Name, expectedPropertyContentBuilder, actualPropertyContentBuilder));
            }
        }

        public static void AreListEqual<T>(IList<T> expected, IList<T> actual)
        {
            AreListEqual<T>(expected, actual, new List<string>());
        }

        public static void AreListEqual<T>(IList<T> expected, IList<T> actual, IList<string> skippedProperties)
        {
            CompareNull(expected, actual);
            if (expected.Count != actual.Count)
            {
                throw new AssertionException(string.Format("Expected list length is {0}, but actual list length is {1}.",
                    expected.Count,
                    actual.Count));
            }

            for (int i = 0; i < expected.Count; i++)
            {
                AreEqual<T>(expected[i], actual[i], skippedProperties, string.Format("index: {0}", i));
            }
        }

        public static void CompareNull(object expected, object actual)
        {
            if (expected == null && actual == null)
            {
                return;
            }

            if (expected == null)
            {
                throw new AssertionException("Expected is null, but actual is not null.");
            }

            if (actual == null)
            {
                throw new AssertionException("Expected is not null, but actual is null.");
            }

        }
    }
}
