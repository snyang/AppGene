﻿using AppGene.Common.Core.Logging;
using AppGene.Common.Entities.Infrastructure.Annotations;
using AppGene.Common.Entities.Infrastructure.Inferences;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace AppGene.Ui.Infrastructure
{

    /// <summary>
    /// Helper class for create UI elements for model
    /// </summary>
    public static class ModelUiCreatorHelper
    {
        public static void CalculatePanelNextPosition(UiLayoutOrientation orientation, int rowsCount, int columnsCount, ref int column, ref int row)
        {
            if (orientation == UiLayoutOrientation.HorizontalThenVertical)
            {
                column += 2;
                if (column >= columnsCount)
                {
                    row++;
                    column = 0;
                }
            }
            else
            {
                row++;
                if (row >= rowsCount)
                {
                    column += 2;
                    row = 0;
                }
            }
        }

        public static LogicalUiElementType DetermineElementType(DisplayPropertyInfo property)
        {
            if (property.PropertyDataType.IsEnum)
            {
                return LogicalUiElementType.Options;
            }

            if (property.PropertyDataType == typeof(DateTime)
                && property.LogicalDataType == LogicalDataType.Date)
            {
                return LogicalUiElementType.Date;
            }

            return LogicalUiElementType.Textbox;
        }

        public static DependencyProperty GetDataGridColumnDependencyProperty(DisplayPropertyInfo property, DependencyObject depObject)
        {
            DependencyProperty dp = null;
            if (property.LogicalDependencyProperty == LogicalDependencyProperty.Default)
            {
                PropertyInfo dpPropertyInfo = depObject.GetType().GetProperty(property.DependencyPropertyName, BindingFlags.Static);
                dp = dpPropertyInfo.GetValue(null) as DependencyProperty;
            }
            else if (property.LogicalDependencyProperty == LogicalDependencyProperty.IsEnabled)
            {
                dp = DataGridColumn.IsReadOnlyProperty;
            }
            else if (property.LogicalDependencyProperty == LogicalDependencyProperty.IsVisible)
            {
                dp = DataGridColumn.VisibilityProperty;
            }
            else if (property.LogicalDependencyProperty == LogicalDependencyProperty.IsReadOnly)
            {
                dp = DataGridColumn.IsReadOnlyProperty;
            }

            if (dp == null)
            {
                LoggerFactory.GetLogger().Warn("The dependency property of property '{}' cannot be found.", property.PropertyName);
            }

            return dp;
        }

        public static DependencyProperty GetDependencyProperty(DisplayPropertyInfo property, DependencyObject depObject)
        {
            DependencyProperty dp = null;
            if (property.LogicalDependencyProperty == LogicalDependencyProperty.Default)
            {
                PropertyInfo dpPropertyInfo = depObject.GetType().GetProperty(property.DependencyPropertyName, BindingFlags.Static);
                dp = dpPropertyInfo.GetValue(null) as DependencyProperty;
            }
            else if (property.LogicalDependencyProperty == LogicalDependencyProperty.IsEnabled)
            {
                dp = UIElement.IsEnabledProperty;
            }
            else if (property.LogicalDependencyProperty == LogicalDependencyProperty.IsVisible)
            {
                dp = UIElement.IsVisibleProperty;
            }
            else if (property.LogicalDependencyProperty == LogicalDependencyProperty.IsReadOnly)
            {
                dp = TextBoxBase.IsReadOnlyProperty;
            }

            if (dp == null)
            {
                LoggerFactory.GetLogger().Warn("The dependency property of property '{}' cannot be found.", property.PropertyName);
            }

            return dp;
        }

        internal static Binding CreateBinding(DisplayPropertyInfo property, string bindingPath)
        {
            IValueConverter converter = null;
            if (!string.IsNullOrEmpty(property.ConverterTypeName))
            {
                converter = Activator.CreateInstance(Type.GetType(property.ConverterTypeName)) as IValueConverter;
            }

            return CreateBinding(property, bindingPath, converter, null);
        }

        internal static Binding CreateBinding(DisplayPropertyInfo property)
        {
            return CreateBinding(property, property.PropertyName);
        }

        internal static Binding CreateBinding(DisplayPropertyInfo property, string bindingPath, IValueConverter converter, string convertParameter)
        {
            var binding = new Binding(bindingPath)
            {
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                Mode = property.IsReadOnly ? BindingMode.OneWay : BindingMode.TwoWay,
                ValidatesOnDataErrors = true,
                NotifyOnValidationError = true,
                ValidatesOnExceptions = true,
            };

            if (converter != null)
            {
                binding.Converter = converter;
                binding.ConverterParameter = convertParameter;
            }

            if (!string.IsNullOrEmpty(property.DisplayFormat))
            {
                binding.StringFormat = property.DisplayFormat;
            }

            return binding;
        }
    }
}
