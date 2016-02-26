using System;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace AppGene.Ui.Infrastructure
{
    public static class UiTool
    {
        public static bool HandleUiEvent(Action action)
        {
            return HandleUiEvent(action, null);
        }

        public static bool HandleUiEvent(Action action,
            Action exceptionRaisingAction)
        {
            Window activeWindow = null;
            try
            {
                activeWindow = Application.Current.Windows.Cast<Window>().SingleOrDefault(x => x.IsActive);
                if (activeWindow != null) activeWindow.Cursor = Cursors.Wait;
                action.Invoke();
            }
            catch (Exception ex)
            {
                string errorMessage = "";
                DbEntityValidationException dbEx = ex as DbEntityValidationException;
                if (dbEx != null)
                {
                    foreach (var errorResult in dbEx.EntityValidationErrors)
                    {
                        foreach (var error in errorResult.ValidationErrors)
                        {
                            errorMessage = error.ErrorMessage;
                        }
                    }
                }
                else
                {
#if (DEBUG)
                    errorMessage = ex.ToString();
#else
                   errorMessage = ex.Message;
#endif
                }

                Debug.WriteLine("Error: " + errorMessage);
                MessageBox.Show("Error: " + errorMessage);
                if (exceptionRaisingAction != null)
                {
                    exceptionRaisingAction.Invoke();
                }
                return false;
            }
            finally
            {
                if (activeWindow != null) activeWindow.Cursor = Cursors.Arrow;
            }

            return true;
        }

        /// <summary>
        /// Create columns for the details grid.
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="columnCount"></param>
        public static void CreateGridColumns(Grid grid,
            int columnCount)
        {
            for (int i = 0; i < columnCount; i++)
            {
                ColumnDefinition column = new ColumnDefinition();
                grid.ColumnDefinitions.Add(column);
                if (i % 2 == 0)
                {
                    column.Width = GridLength.Auto;
                }
            }
        }

        public static void CreateCommandGridColumns(Grid grid,
            int columnCount)
        {
            for (int i = 0; i < columnCount; i++)
            {
                ColumnDefinition column = new ColumnDefinition();
                grid.ColumnDefinitions.Add(column);
                if (i == 0)
                {
                    column.Width = new GridLength(1, GridUnitType.Star);
                }
                else
                {
                    column.Width = GridLength.Auto;
                }
            }
        }

        /// <summary>
        /// Create rows for the details grid.
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="rowCount"></param>
        public static void CreateGridRows(Grid grid,
            int rowCount)
        {
            for (int i = 0; i < rowCount; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition());
            }
        }

        /// <summary>
        /// Create fields for the details grid.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="label"></param>
        /// <param name="bindingPath"></param>
        /// <param name="row"></param>
        /// <param name="column"></param>
        public static void CreateField(Grid grid,
            String name,
            String label,
            String bindingPath,
            Style style,
            int row,
            int column)
        {
            Label labelElement = new Label
            {
                Name = "label" + name,
                //TODO: localization ":"
                Content = label + ":"
            };
            Grid.SetRow(labelElement, row);
            Grid.SetColumn(labelElement, column);

            TextBox textBox = new TextBox
            {
                Name = "textBox" + name,
            };
            if (style != null)
            {
                textBox.Style = style;
            }
            textBox.SetBinding(TextBox.TextProperty, new Binding(bindingPath)
            {
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                Mode = BindingMode.TwoWay,
                ValidatesOnDataErrors = true,
                NotifyOnValidationError = true,
                ValidatesOnExceptions = true,
            });

            Grid.SetRow(textBox, row);
            Grid.SetColumn(textBox, checked(column + 1));

            grid.Children.Add(labelElement);
            grid.Children.Add(textBox);
        }

        public static void CreateEnumField(Grid parent,
            Type enumType,
            String name,
            String label,
            String bindingPath,
            Style style,
            int row,
            int column)
        {
            // create label
            Label labelElement = new Label
            {
                Name = "label" + name,
                Content = label + ":"
            };
            Grid.SetRow(labelElement, row);
            Grid.SetColumn(labelElement, column);
            parent.Children.Add(labelElement);

            // create inputs
            if (!enumType.IsEnum) return;
            StackPanel panel = new StackPanel()
            {
                Orientation = Orientation.Horizontal,
                Margin = new Thickness(4),
            };
            Grid.SetRow(panel, row);
            Grid.SetColumn(panel, checked(column + 1));
            parent.Children.Add(panel);

            Array enumValues = Enum.GetValues(enumType);
            foreach (var item in enumValues)
            {
                RadioButton control = new RadioButton
                {
                    Name = "radioButton" + name + "_" + item.ToString(),
                    Content = item.ToString(),
                    GroupName = name,
                    Margin = new Thickness(4),
                };
                if (style != null)
                {
                    control.Style = style;
                }
                control.SetBinding(RadioButton.IsCheckedProperty, new Binding(bindingPath)
                {
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                    Mode = BindingMode.TwoWay,
                    Converter = new EnumToBooleanConverter(),
                    ConverterParameter = item.ToString()
                });

                panel.Children.Add(control);
            }
        }

        public static void CreateDateField(Grid grid,
            String name,
            String label,
            String bindingPath,
            Style style,
            int row,
            int column)
        {
            Label labelElement = new Label
            {
                Name = "label" + name,
                Content = label + ":"
            };
            Grid.SetRow(labelElement, row);
            Grid.SetColumn(labelElement, column);

            DatePicker control = new DatePicker
            {
                Name = "datePicker" + name
            };
            if (style != null)
            {
                control.Style = style;
            }

            control.SetBinding(DatePicker.SelectedDateProperty, new Binding(bindingPath)
            {
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                Mode = BindingMode.TwoWay,
                ValidatesOnDataErrors = true,
                NotifyOnValidationError = true,
                ValidatesOnExceptions = true,
            });

            Grid.SetRow(control, row);
            Grid.SetColumn(control, checked(column + 1));

            grid.Children.Add(labelElement);
            grid.Children.Add(control);
        }

        public static Button CreateGridCommand(Grid grid,
                            String name,
                            String content,
                            int column)
        {
            Button button = new Button
            {
                Name = name,
                Content = content,
                Margin = new Thickness(4, 0, 0, 0),
                Height = 22,
                MinWidth = 75
            };
            Grid.SetColumn(button, column);

            grid.Children.Add(button);

            return button;
        }
    }
}