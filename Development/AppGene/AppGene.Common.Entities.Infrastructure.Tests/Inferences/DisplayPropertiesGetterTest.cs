using AppGene.Common.Entities.Infrastructure.Annotations;
using AppGene.Common.Entities.Infrastructure.Inferences;
using AppGene.Common.Entities.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace AppGene.Common.Entities.Infrastructure.Tests.Inferences
{
    [TestFixture]
    public class DisplayPropertiesGetterTest
    {
        [Test]
        public void TestEntity()
        {
            var displayProperties = new DisplayPropertiesGetter().GetProperties(new EntityAnalysisContext
            {
                EntityType = typeof(Employee),
                Source = this.GetType().FullName,
            });

            ObjectComparator.AreListEqual<DisplayPropertyInfo>(
                GetEmployeValidateDisplayProperties(),
                displayProperties,
                new List<string>(new string[] { "PropertyInfo" }));
        }

        [Test]
        public void TestEntityModel()
        {
            var displayProperties = new DisplayPropertiesGetter().GetProperties(new EntityAnalysisContext
            {
                EntityType = typeof(EmployeeModel),
                Source = this.GetType().FullName,
            });

            ObjectComparator.AreListEqual<DisplayPropertyInfo>(
                GetEmployeeModelValidateDisplayProperties(),
                displayProperties,
                new List<string>(new string[] { "PropertyInfo" }));
        }

        private IList<DisplayPropertyInfo> GetEmployeValidateDisplayProperties()
        {
            var properties = new List<DisplayPropertyInfo>();
            int order = 1;

            properties.Add(new DisplayPropertyInfo
            {
                Name = "Employee Id",
                ShortName = "Id",
                IsHidden = true,
                Order = order,
            });

            order++;
            properties.Add(new DisplayPropertyInfo
            {
                Name = "Employee Code",
                ShortName = "Code",
                Order = order,
                DisplayFormat = "0000"
            });

            order++;
            properties.Add(new DisplayPropertyInfo
            {
                Name = "Employee Name",
                ShortName = "Name",
                Order = order,
                ContentLength = 50,
                Description = "",
                Prompt = "<employee name>",
            });

            order++;
            properties.Add(new DisplayPropertyInfo
            {
                Name = "Gender",
                ShortName = "Gender",
                Order = order,
            });

            order++;
            properties.Add(new DisplayPropertyInfo
            {
                Name = "Birthday",
                ShortName = "Birthday",
                Order = order,
                LogicalDataType = LogicalDataType.Date,
            });

            order++;
            properties.Add(new DisplayPropertyInfo
            {
                Name = "On Board Date",
                ShortName = "On Board Date",
                Order = order,
                ConverterTypeName = "AppGene.Ui.Infrastructure.Converters.Int32ToDateConverter",
                LogicalDataType = LogicalDataType.Date,
                CustomType = typeof(DateTime),
            });

            order++;
            properties.Add(new DisplayPropertyInfo
            {
                Name = "Female Benefit",
                ShortName = "Female Benefit",
                Order = order,
                DisplayFormat = "#,##0.00"
            });


            return properties;
        }

        private IList<DisplayPropertyInfo> GetEmployeeModelValidateDisplayProperties()
        {
            var properties = new List<DisplayPropertyInfo>();
            int order = 1;

            properties.Add(new DisplayPropertyInfo
            {
                Name = "Employee Id",
                ShortName = "Id",
                IsHidden = true,
                Order = order,
            });

            order++;
            properties.Add(new DisplayPropertyInfo
            {
                Name = "Employee Code",
                ShortName = "Code",
                Order = order,
                DisplayFormat = "0000"
            });

            order++;
            properties.Add(new DisplayPropertyInfo
            {
                Name = "Employee Name",
                ShortName = "Name",
                Order = order,
                ContentLength = 50,
                Description = "",
                Prompt = "<employee name>",
            });

            order++;
            properties.Add(new DisplayPropertyInfo
            {
                Name = "Gender",
                ShortName = "Gender",
                Order = order,
            });

            order++;
            properties.Add(new DisplayPropertyInfo
            {
                Name = "Birthday",
                ShortName = "Birthday",
                Order = order,
                LogicalDataType = LogicalDataType.Date,
            });

            order++;
            properties.Add(new DisplayPropertyInfo
            {
                Name = "Age",
                ShortName = "Age",
                Order = order,
                IsComputed = true,
                ComputeReferencePropertyNames = new string[] { "Birthday" },
                IsReadOnly = true,
            });

            order++;
            properties.Add(new DisplayPropertyInfo
            {
                Name = "On Board Date",
                ShortName = "On Board Date",
                Order = order,
                ConverterTypeName = "AppGene.Ui.Infrastructure.Converters.Int32ToDateConverter",
                LogicalDataType = LogicalDataType.Date,
                CustomType = typeof(DateTime),
            });

            order++;
            properties.Add(new DisplayPropertyInfo
            {
                Name = "Female Benefit",
                ShortName = "Female Benefit",
                Order = order,
                DisplayFormat = "#,##0.00"
            });

            order++;
            properties.Add(new DisplayPropertyInfo
            {
                Name = "Female Benefit Is Read Only",
                ShortName = "Female Benefit Is Read Only",
                Order = -1,
                ComputeReferencePropertyNames = new string[] { "Gender" },
                IsComputed = true,
                IsReadOnly = true,
                IsDependencyProperty = true,
                DependencyHostPropertyName = "FemaleBenefit",
                LogicalDependencyProperty= LogicalDependencyProperty.IsReadOnly
            });

            return properties;
        }
    }
}
