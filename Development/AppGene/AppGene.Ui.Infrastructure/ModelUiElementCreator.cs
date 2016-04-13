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
            if (elementType == LogicalUiElementType.Boolean)
            {
                return CreateBooleanField(parent,
                          property,
                          bindingPathPrefix + "/" + property.PropertyName,
                          null,
                          row,
                          column);
            }
            else if (elementType == LogicalUiElementType.ComboBox)
            {
                return CreateComboBoxField(parent,
                          property,
                          bindingPathPrefix + "/" + property.PropertyName,
                          null,
                          row,
                          column);
            }
            else if(elementType == LogicalUiElementType.Date)
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

            (uiElementsInfo.Content as FrameworkElement).SetBinding(dp,
                ModelUiCreatorHelper.CreateBinding(property, bindingPathPrefix + "/" + property.PropertyName));
        }

        private static ModelPropertyUiInfo CreateBooleanField(Grid parent,
                    DisplayPropertyInfo property,
            String bindingPath,
            Style style,
            int row,
            int column)
        {
            Label labelElement = CreateLabel(property, row, column);

            var checkBox = new CheckBox
            {
                Name = "checkBox" + property.PropertyName,
            };
            if (style != null)
            {
                checkBox.Style = style;
            }
            checkBox.VerticalAlignment = VerticalAlignment.Center;
            checkBox.SetBinding(ToggleButton.IsCheckedProperty, ModelUiCreatorHelper.CreateBinding(property, bindingPath));

            if (property.IsReadOnly)
            {
                checkBox.IsEnabled = false;
            }

            Grid.SetRow(checkBox, row);
            Grid.SetColumn(checkBox, checked(column + 1));

            parent.Children.Add(labelElement);
            parent.Children.Add(checkBox);

            // return
            ModelPropertyUiInfo elememtsInfo = new ModelPropertyUiInfo(property);
            elememtsInfo.Label = labelElement;
            elememtsInfo.Content = checkBox;
            return elememtsInfo;
        }

        private static ModelPropertyUiInfo CreateComboBoxField(Grid parent,
            DisplayPropertyInfo property,
            String bindingPath,
            Style style,
            int row,
            int column)
        {
            Label labelElement = CreateLabel(property, row, column);

            var comboBox = new ComboBox
            {
                Name = "comboBox" + property.PropertyName,
            };
            if (style != null)
            {
                comboBox.Style = style;
            }
            comboBox.SetBinding(Selector.SelectedItemProperty, ModelUiCreatorHelper.CreateBinding(property, bindingPath));

            if (property.IsReadOnly)
            {
                comboBox.IsReadOnly = true;
            }

            ObjectDataProvider provider = new ObjectDataProvider()
            {
                ObjectType = typeof(Enum),
                MethodName = "GetValues",
            };
            provider.MethodParameters.Add(property.PropertyInfo.PropertyType);

            BindingOperations.SetBinding(comboBox, ItemsControl.ItemsSourceProperty, new Binding()
            {
                Source = provider
            });

            Grid.SetRow(comboBox, row);
            Grid.SetColumn(comboBox, checked(column + 1));

            parent.Children.Add(labelElement);
            parent.Children.Add(comboBox);

            // return
            ModelPropertyUiInfo elememtsInfo = new ModelPropertyUiInfo(property);
            elememtsInfo.Label = labelElement;
            elememtsInfo.Content = comboBox;
            return elememtsInfo;
        }

        private static ModelPropertyUiInfo CreateDateField(Grid grid,
            DisplayPropertyInfo property,
            String bindingPath,
            Style style,
            int row,
            int column)
        {
            Label labelElement = CreateLabel(property, row, column);

            DatePicker control = new DatePicker
            {
                Name = "datePicker" + property.PropertyName
            };
            if (style != null)
            {
                control.Style = style;
            }

            Binding binding = ModelUiCreatorHelper.CreateBinding(property, bindingPath);
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
            Label labelElement = CreateLabel(property, row, column);
            parent.Children.Add(labelElement);
            elememtsInfo.Label = labelElement;

            // create inputs
            var panel = new WrapPanel()
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
                control.SetBinding(ToggleButton.IsCheckedProperty,
                    ModelUiCreatorHelper.CreateBinding(property, bindingPath, new EnumToBooleanConverter(), item.ToString()));

                if (property.IsReadOnly)
                {
                    control.IsEnabled = false;
                }

                panel.Children.Add(control);
            }

            return elememtsInfo;
        }

        private static Label CreateLabel(DisplayPropertyInfo property, int row, int column)
        {
            Label labelElement = new Label
            {
                Name = "label" + property.PropertyName,
                //TODO: localization ":"
                Content = property.Name + ":"
            };
            Grid.SetRow(labelElement, row);
            Grid.SetColumn(labelElement, column);
            return labelElement;
        }
 
        private static ModelPropertyUiInfo CreateTextBoxField(Grid parent,
            DisplayPropertyInfo property,
            String bindingPath,
            Style style,
            int row,
            int column)
        {
            Label labelElement = CreateLabel(property, row, column);

            TextBox textBox = new TextBox
            {
                Name = "textBox" + property.PropertyName,
            };
            if (style != null)
            {
                textBox.Style = style;
            }
            textBox.SetBinding(TextBox.TextProperty, ModelUiCreatorHelper.CreateBinding(property, bindingPath));

            if (property.IsReadOnly)
            {
                textBox.IsReadOnly = true;
            }

            SetInputBehavior(property, textBox);

            if (ModelUiCreatorHelper.IsNumericType(property.PropertyDataType))
            {
                textBox.HorizontalContentAlignment = HorizontalAlignment.Right;
            }
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
            if (ModelUiCreatorHelper.IsNumericType(propertyType))
            {
                element.SetValue(DigitsOnlyBehavior.IsDigitOnlyProperty, true);
            }
        }
    }
}