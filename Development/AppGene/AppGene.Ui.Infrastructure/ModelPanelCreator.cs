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
    public class ModelPanelCreator
    {
        public int ColumnsCount { get; set; } = 2;

        public UiLayoutOrientation Orientation { get; set; } = UiLayoutOrientation.VerticalThenHorizontal;

        public Grid DetailGrid { get; set; }

        public ModelUiInfo Create(IList<DisplayPropertyInfo> editProperties, int gridRow, Style style, string bindingPathPrefix)
        {
            DetailGrid = new Grid
            {
                Margin = new Thickness(4)
            };
            Grid.SetRow(DetailGrid, gridRow);

            int totalVisibleProperties = editProperties.Count((p) => p.IsHidden == false);
            int rowsCount = (int)Math.Ceiling(((decimal)totalVisibleProperties) / 2);

            // create columns
            CreateGridColumns(DetailGrid, ColumnsCount * 2);

            // create rows
            CreateGridRows(DetailGrid, rowsCount);

            // Create Fields
            int row = 0;
            int column = 0;

            var modelUiInfo = new ModelUiInfo();
            modelUiInfo.Grid = DetailGrid;
            foreach (var property in editProperties)
            {
                if (property.IsHidden && !property.IsDependencyProperty) continue;

                if (!property.IsDependencyProperty)
                { 
                    var UiElementsInfo = ModelUiElementCreator.Create(property,
                        DetailGrid,
                        row,
                        column,
                        bindingPathPrefix,
                        style);
                    modelUiInfo.PropertyUiInfos.Add(UiElementsInfo.PropertyName, UiElementsInfo);

                    ModelUiCreatorHelper.CalculatePanelNextPosition(Orientation, rowsCount, ColumnsCount, ref column, ref row);
                }
                else
                {
                    var UiElementsInfo = modelUiInfo.PropertyUiInfos[property.DependencyHostPropertyName];
                    ModelUiElementCreator.CreateDependencyProperty(UiElementsInfo, property, bindingPathPrefix);
                }
            }

            return modelUiInfo;
        }

        /// <summary>
        /// Create columns for the details grid.
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="columnCount"></param>
        private static void CreateGridColumns(Grid grid,
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

        /// <summary>
        /// Create rows for the details grid.
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="rowCount"></param>
        private static void CreateGridRows(Grid grid,
            int rowCount)
        {
            for (int i = 0; i < rowCount; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition());
            }
        }
    }
}
