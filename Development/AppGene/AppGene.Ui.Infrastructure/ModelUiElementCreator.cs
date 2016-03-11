using AppGene.Common.Core.Logging;
using AppGene.Common.Entities.Infrastructure.Annotations;
using AppGene.Common.Entities.Infrastructure.Inferences;
using AppGene.Ui.Infrastructure.Converters;
using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace AppGene.Ui.Infrastructure
{
    public static class ModelUiElementCreator
    {
        // TODO: Enrich the class PropertyControlCreator
        public static ModelPropertyUiInfo Create(DisplayPropertyInfo property,
            Grid parent,
            int row,
            int column,
            string bindingPathPrefix,
            Style style)
        {
            LogicalUiElementType elementType = ModelUiCreatorHelper.DetermineElementType(property);
            if (elementType == LogicalUiElementType.Date)
            {
                return CreateDateField(parent,
                          property,
                          bindingPathPrefix + "/" + property.PropertyName,
                          null,
                          row,
                          column);
            }
            else if (elementType == LogicalUiElementType.Options)
            {
                return CreateEnumField(parent,
                            property.PropertyInfo.PropertyType,
                            property,
                            bindingPathPrefix + "/" + property.PropertyName,
                            style,
                            row,
                            column);
            }

            return CreateTextBoxField(parent,
                        property,
                        bindingPathPrefix + "/" + property.PropertyName,
                        style,
                        row,
                        column);
        }

        internal static void CreateDependencyProperty(ModelPropertyUiInfo uiElementsInfo,
            DisplayPropertyInfo property, 
            string bindingPathPrefix)
        {
            DependencyProperty dp = ModelUiCreatorHelper.GetDependencyProperty(property, uiElementsInfo.Content);

            if (dp == null)
            {
                return;
            }

            (uiElementsInfo.Content as FrameworkElement).SetBinding(dp, new Binding(bindingPathPrefix + "/" + property.PropertyName)
            {
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                Mode = property.IsReadOnly ? BindingMode.OneWay : BindingMode.TwoWay,
                ValidatesOnDataErrors = true,
                NotifyOnValidationError = true,
                ValidatesOnExceptions = true,
            });
        }

        /// <summary>
        /// Creates UI element for specific property.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="property"></param>
        /// <param name="bindingPath"></param>
        /// <param name="style"></param>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        private static ModelPropertyUiInfo CreateTextBoxField(Grid parent,
            DisplayPropertyInfo property,
            String bindingPath,
            Style style,
            int row,
            int column)
        {
            Label labelElement = new Label
            {
                Name = "label" + property.PropertyName,
                //TODO: localization ":"
                Content = property.Name + ":"
            };
            Grid.SetRow(labelElement, row);
            Grid.SetColumn(labelElement, column);

            TextBox textBox = new TextBox
            {
                Name = "textBox" + property.PropertyName,
            };
            if (style != null)
            {
                textBox.Style = style;
            }
            textBox.SetBinding(TextBox.TextProperty, new Binding(bindingPath)
            {
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                Mode = property.IsReadOnly ? BindingMode.OneWay : BindingMode.TwoWay,
                ValidatesOnDataErrors = true,
                NotifyOnValidationError = true,
                ValidatesOnExceptions = true,
            });

            if (property.IsReadOnly)
            {
                textBox.IsReadOnly = true;
            }

            SetInputBehavior(property, textBox);

            // display caret when readonly 
            textBox.IsReadOnlyCaretVisible = true;

            Grid.SetRow(textBox, row);
            Grid.SetColumn(textBox, checked(column + 1));

            parent.Children.Add(labelElement);
            parent.Children.Add(textBox);

            // return
            ModelPropertyUiInfo elememtsInfo = new ModelPropertyUiInfo(property);
            elememtsInfo.Label = labelElement;
            elememtsInfo.Content = textBox;
            return elememtsInfo;
        }

        private static void SetInputBehavior(DisplayPropertyInfo property, DependencyObject element)
        {
            // TODO: Enhancement it to support nullable, Uxxx, decimal etc.
            Type propertyType = property.PropertyInfo.PropertyType;
            if (IsNumericType(propertyType))
            {
                element.SetValue(DigitsOnlyBehavior.IsDigitOnlyProperty, true);
            }
        }

        private static bool IsNullable(Type type)
        {
            return
                type != null &&
                type.IsGenericType &&
                type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        private static bool IsNumericType(Type type)
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

        private static ModelPropertyUiInfo CreateEnumField(Grid parent,
            Type enumType,
            DisplayPropertyInfo property,
            String bindingPath,
            Style style,
            int row,
            int column)
        {
            ModelPropertyUiInfo elememtsInfo = new ModelPropertyUiInfo(property);

            // create label
            Label labelElement = new Label
            {
                Name = "label" + property.PropertyName,
                Content = property.Name + ":"
            };
            Grid.SetRow(labelElement, row);
            Grid.SetColumn(labelElement, column);
            parent.Children.Add(labelElement);

            elememtsInfo.Label = labelElement;

            // create inputs
            StackPanel panel = new StackPanel()
            {
                Orientation = Orientation.Horizontal,
                Margin = new Thickness(4),
            };
            Grid.SetRow(panel, row);
            Grid.SetColumn(panel, checked(column + 1));
            parent.Children.Add(panel);
            elememtsInfo.Content = panel;

            if (!enumType.IsEnum) return elememtsInfo;
            Array enumValues = Enum.GetValues(enumType);
            foreach (var item in enumValues)
            {
                RadioButton control = new RadioButton
                {
                    Name = "radioButton" + property.PropertyName + "_" + item.ToString(),
                    // TODO: Localization for enum values
                    Content = item.ToString(),
                    GroupName = property.PropertyName,
                    Margin = new Thickness(4),
                };
                if (style != null)
                {
                    control.Style = style;
                }
                control.SetBinding(RadioButton.IsCheckedProperty, new Binding(bindingPath)
                {
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                    Mode = property.IsReadOnly ? BindingMode.OneWay : BindingMode.TwoWay,
                    Converter = new EnumToBooleanConverter(),
                    ConverterParameter = item.ToString()
                });

                if (property.IsReadOnly)
                {
                    control.IsEnabled = false;
                }

                panel.Children.Add(control);
            }

            return elememtsInfo;
        }

        private static ModelPropertyUiInfo CreateDateField(Grid grid,
            DisplayPropertyInfo property,
            String bindingPath,
            Style style,
            int row,
            int column)
        {
            Label labelElement = new Label
            {
                Name = "label" + property.PropertyName,
                Content = property.Name + ":"
            };
            Grid.SetRow(labelElement, row);
            Grid.SetColumn(labelElement, column);

            DatePicker control = new DatePicker
            {
                Name = "datePicker" + property.PropertyName
            };
            if (style != null)
            {
                control.Style = style;
            }

            var binding = new Binding(bindingPath)
            {
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                Mode = property.IsReadOnly ? BindingMode.OneWay : BindingMode.TwoWay,
                ValidatesOnDataErrors = true,
                NotifyOnValidationError = true,
                ValidatesOnExceptions = true,
            };

            if (!string.IsNullOrEmpty(property.ConverterTypeName))
            {
                binding.Converter = Activator.CreateInstance(Type.GetType(property.ConverterTypeName)) as IValueConverter;
            }
            control.SetBinding(DatePicker.SelectedDateProperty, binding);

            if (property.IsReadOnly)
            {
                control.IsEnabled = false;
            }

            Grid.SetRow(control, row);
            Grid.SetColumn(control, checked(column + 1));

            grid.Children.Add(labelElement);
            grid.Children.Add(control);

            // return
            ModelPropertyUiInfo elememtsInfo = new ModelPropertyUiInfo(property);
            elememtsInfo.Label = labelElement;
            elememtsInfo.Content = control;
            return elememtsInfo;
        }
    }
}