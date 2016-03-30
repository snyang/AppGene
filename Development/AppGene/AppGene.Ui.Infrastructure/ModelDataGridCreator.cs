using AppGene.Common.Entities.Infrastructure.Inferences;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace AppGene.Ui.Infrastructure
{
    public class ModelDataGridCreator
    {
        public ModelDataGridUiInfo Create(IList<DisplayPropertyInfo> editProperties, Style style)
        {
            var DataGrid = new DataGrid
            {
                Margin = new Thickness(2)
            };

            var modelUiInfo = new ModelDataGridUiInfo()
            {
                Grid = DataGrid,
            };

            try
            {
                Grid.SetRow(DataGrid, 1);

                DataGrid.BeginInit();
                DataGrid.AutoGenerateColumns = false;
                DataGrid.IsSynchronizedWithCurrentItem = true;
                DataGrid.Columns.Clear();

                // create data grid columns.

                foreach (var property in editProperties)
                {
                    if (property.IsHidden && !property.IsDependencyProperty) continue;

                    if (!property.IsDependencyProperty)
                    {
                        DataGridColumn column = ModelDataGridColumnCreator.Create(property, style);
                        DataGrid.Columns.Add(column);
                        modelUiInfo.Columns.Add(property.PropertyName, column);
                    }
                    else
                    {
                        var column = modelUiInfo.Columns[property.DependencyHostPropertyName];
                        ModelDataGridColumnCreator.CreateDependencyProperty(column, property);
                    }
                }
            }
            finally
            {
                DataGrid.EndInit();
            }

            return modelUiInfo;
        }
    }
}
