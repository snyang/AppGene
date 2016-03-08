using AppGene.Common.Entities.Infrastructure.Inferences;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace AppGene.Ui.Infrastructure
{
    public static class ModelDataGridColumnCreator
    {
        // TODO: Enrich the class PropertyDataGridColumnCreator
        public static DataGridColumn Create(DisplayPropertyInfo property, Style style)
        {
            if (property.PropertyInfo.PropertyType.IsEnum)
            {
                return CreateEnumColumn(property, style);
            }
            if (property.PropertyInfo.PropertyType == typeof(DateTime))
            {
                return CreateDateColumn(property, style);
            }

            return CreateTextColumn(property, style);
        }

        private static DataGridColumn CreateTextColumn(DisplayPropertyInfo property, Style style)
        {
            var column = new DataGridTextColumn
            {
                Header = property.ShortName,
                EditingElementStyle = style,
                ElementStyle = style,
                Binding = new Binding(property.PropertyName)
                {
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                    Mode = BindingMode.TwoWay,
                    ValidatesOnDataErrors = true,
                    NotifyOnValidationError = true,
                    ValidatesOnExceptions = true,
                }
            };
            if (!string.IsNullOrEmpty(property.DisplayFormat))
            {
                column.Binding.StringFormat = property.DisplayFormat;
            }
            column.IsReadOnly = property.IsReadOnly;

            return column;
        }

        private static DataGridColumn CreateEnumColumn(DisplayPropertyInfo property, Style style)
        {
            // Gender
            // https://msdn.microsoft.com/zh-cn/library/system.windows.controls.datagridcomboboxcolumn.aspx
            ObjectDataProvider provider = new ObjectDataProvider()
            {
                ObjectType = typeof(Enum),
                MethodName = "GetValues",
            };
            provider.MethodParameters.Add(property.PropertyInfo.PropertyType);

            DataGridComboBoxColumn column = new DataGridComboBoxColumn
            {
                Header = property.ShortName,
                EditingElementStyle = style,
                ElementStyle = style,
                SelectedItemBinding = new Binding(property.PropertyName)
                {
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                    Mode = BindingMode.TwoWay,
                    ValidatesOnDataErrors = true,
                    NotifyOnValidationError = true,
                    ValidatesOnExceptions = true,
                },
            };

            //http://stackoverflow.com/questions/25644003/wpf-datagridcomboboxcolumn-binding
            BindingOperations.SetBinding(column, DataGridComboBoxColumn.ItemsSourceProperty, new Binding()
            {
                Source = provider
            });

            return column;
        }

        private static DataGridColumn CreateDateColumn(DisplayPropertyInfo property, Style style)
        {
            // Column Birthday Binding
            Binding binding = new Binding(property.PropertyName)
            {
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                Mode = BindingMode.TwoWay,
                ValidatesOnDataErrors = true,
                NotifyOnValidationError = true,
                ValidatesOnExceptions = true,
                StringFormat = "d"
            };
            if (!string.IsNullOrEmpty(property.DisplayFormat))
            {
                binding.StringFormat = property.DisplayFormat;
            }

            // Column Birthday
            var column = new DataGridTemplateColumn
            {
                Header = property.ShortName,
                CellTemplate = CreateDataTemplate<TextBlock>(TextBlock.TextProperty, binding),
                CellEditingTemplate = CreateDataTemplate<DatePicker>(DatePicker.TextProperty, binding),
                CellStyle = style,
            };

            return column;
        }

        private static DataTemplate CreateDataTemplate<T>(DependencyProperty bindingProperty, Binding binding)
        {
            FrameworkElementFactory cellElement = new FrameworkElementFactory(typeof(T));
            cellElement.SetBinding(bindingProperty, binding);

            // Column Birthday CellTemplate
            DataTemplate cellTemplate = new DataTemplate(typeof(T))
            {
                VisualTree = cellElement,
            };

            return cellTemplate;
        }
    }
}