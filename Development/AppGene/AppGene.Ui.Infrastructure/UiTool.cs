using AppGene.Ui.Infrastructure.Converters;
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