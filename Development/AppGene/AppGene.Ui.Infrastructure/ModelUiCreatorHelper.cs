using AppGene.Common.Core.Logging;
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
                if (property.PropertyDataType.GetEnumNames().Length <= 3)
                {
                    return LogicalUiElementType.Options;
                }
                else
                {
                    return LogicalUiElementType.ComboBox;
                }
            }

            if (property.PropertyDataType == typeof(DateTime)
                && property.LogicalDataType == LogicalDataType.Date)
            {
                return LogicalUiElementType.Date;
            }

            if (property.PropertyDataType == typeof(Boolean))
            {
                return LogicalUiElementType.Boolean;
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

            if (ModelUiCreatorHelper.IsNullable(property.PropertyDataType))
            {
                binding.TargetNullValue = "";
            }

            return binding;
        }

        public static bool IsNullable(Type type)
        {
            return type != null
                && type.IsGenericType
                && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        public static bool IsNumericType(Type type)
        {
            if (type == null || type.IsEnum)
                return false;

            if (IsNullable(type))
                return IsNumericType(Nullable.GetUnderlyingType(type));

            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Byte:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.SByte:
                case TypeCode.Single:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                    return true;
                default:
                    return false;
            }
        }
    }
}
