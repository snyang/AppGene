using AppGene.Common.Entities.Infrastructure.Inferences;
using AppGene.Common.Entities.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using AppGene.Common.Entities;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace AppGene.Ui.Infrastructure.Tests
{

    public static class ModellCreatorTestHelper
    {
        public static void ValidateBinding(DisplayPropertyInfo property, Binding binding, string message)
        {
            ValidateBinding(property, binding, property.PropertyName, message);
        }

        public static void ValidateBinding(DisplayPropertyInfo property, Binding binding, string bindingPath, string message)
        {
            Assert.AreEqual(bindingPath, binding.Path.Path, message);
            Assert.AreEqual(property.IsReadOnly
                    ? BindingMode.OneWay
                    : BindingMode.TwoWay,
                    binding.Mode, message);
            if (!string.IsNullOrEmpty(property.ConverterTypeName))
            {
                Assert.AreEqual(property.ConverterTypeName, binding.Converter.GetType().FullName, message);
            }
            if (!string.IsNullOrEmpty(property.DisplayFormat))
            {
                Assert.AreEqual(property.DisplayFormat, binding.StringFormat, message);
            }
            if (ModelUiCreatorHelper.IsNullable(property.PropertyDataType))
            {
                Assert.IsNotNull(binding.TargetNullValue, message);
            }
        }
    }
}
