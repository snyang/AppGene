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
        public DataGrid DataGridMain { get; set; }

        public DataGrid Create(IList<DisplayPropertyInfo> editProperties, Style style)
        {
            try
            {
                DataGridMain = new DataGrid
                {
                    Margin = new Thickness(2)
                };
                Grid.SetRow(DataGridMain, 1);

                DataGridMain.BeginInit();
                DataGridMain.AutoGenerateColumns = false;
                DataGridMain.IsSynchronizedWithCurrentItem = true;
                DataGridMain.Columns.Clear();

                // create data grid columns.
                var modelUiInfo = new Dictionary<string, DataGridColumn>();
                foreach (var property in editProperties)
                {
                    if (property.IsHidden && !property.IsDependencyProperty) continue;

                    if (!property.IsDependencyProperty)
                    {
                        DataGridColumn column = ModelDataGridColumnCreator.Create(property, style);
                        DataGridMain.Columns.Add(column);
                        modelUiInfo.Add(property.PropertyName, column);
                    }
                    else
                    {
                        var column = modelUiInfo[property.DependencyHostPropertyName];
                        ModelDataGridColumnCreator.CreateDependencyProperty(column, property);
                    }
                }
            }
            finally
            {
                DataGridMain.EndInit();
            }

            return DataGridMain;
        }
    }
}
