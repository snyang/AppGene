using AppGene.Common.Entities.Infrastructure.Inferences;
using AppGene.Common.Entities.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Data;
using AppGene.Common.Entities;

namespace AppGene.Ui.Infrastructure.Tests
{
    [Apartment(ApartmentState.STA)]
    [TestFixture(typeof(EmployeeModel))]
    [TestFixture(typeof(DataTypeGroupA))]
    public class ModelDataGridCreatorTest<TModel>
    {
        [Test]
        public void TestCreateGrid()
        {
            var creator = new ModelDataGridCreator();
            IList<DisplayPropertyInfo> displayProperties = new DisplayPropertiesGetter().GetProperties(new EntityAnalysisContext()
            {
                EntityType = typeof(TModel)
            });
            Window testWindow = new Window();
            ModelDataGridUiInfo modelUiInfo = creator.Create(displayProperties, null);
            testWindow.Content = modelUiInfo.Grid;
            ValidateCreateGrid(modelUiInfo, displayProperties);
        }

        private void ValidateColumnHeader(DataGridColumn gridColumn, string header, int column)
        {
            Assert.AreEqual(header, gridColumn.Header, $"property = {header}");
        }

        private void ValidateColumntDate(DataGridColumn gridColumn, DisplayPropertyInfo property)
        {
            if (string.IsNullOrEmpty(property.DisplayFormat))
            {
                property.DisplayFormat = "d";
            }

            Assert.IsTrue(gridColumn is DataGridTemplateColumn, $"property = {property.PropertyName}");
            DataGridTemplateColumn templateColumn = (DataGridTemplateColumn)gridColumn;

            templateColumn.CellTemplate.Seal();
            var cell = templateColumn.CellTemplate.LoadContent() as TextBlock;
            Assert.IsNotNull(cell, $"property = {property.PropertyName}");
            ModellCreatorTestHelper.ValidateBinding(property, 
                BindingOperations.GetBinding(cell, TextBlock.TextProperty),
                $"property = {property.PropertyName}");

            templateColumn.CellEditingTemplate.Seal();
            var cellEditing = templateColumn.CellEditingTemplate.LoadContent() as DatePicker;
            Assert.IsNotNull(cellEditing, $"property = {property.PropertyName}");
            ModellCreatorTestHelper.ValidateBinding(property, 
                BindingOperations.GetBinding(cellEditing, DatePicker.TextProperty),
                $"property = {property.PropertyName}");
        }

        private void ValidateColumntEnum(DataGridColumn gridColumn, DisplayPropertyInfo property)
        {
            Assert.IsTrue(gridColumn is DataGridComboBoxColumn, $"property = {property.PropertyName}");
            DataGridComboBoxColumn column = (DataGridComboBoxColumn)gridColumn;

            ModellCreatorTestHelper.ValidateBinding(property, 
                column.SelectedItemBinding as Binding,
                $"property = {property.PropertyName}");
        }

        private void ValidateColumnTextBox(DataGridColumn gridColumn, DisplayPropertyInfo property)
        {
            Assert.IsTrue(gridColumn is DataGridTextColumn, $"property = {property.PropertyName}");
            DataGridTextColumn textColumn = (DataGridTextColumn)gridColumn;
            ModellCreatorTestHelper.ValidateBinding(property, 
                textColumn.Binding as Binding,
                $"property = {property.PropertyName}");
        }

        private void ValidateCreateGrid(ModelDataGridUiInfo modelUiInfo, IList<DisplayPropertyInfo> displayProperties)
        {
            ValidateGridColumns(modelUiInfo, displayProperties);

            ValidateDependencyProperties(modelUiInfo, displayProperties);
        }

        private void ValidateGridColumns(ModelDataGridUiInfo modelUiInfo, IList<DisplayPropertyInfo> displayProperties)
        {
            var grid = modelUiInfo.Grid;
            int propertiesCount = displayProperties.Count(p => p.IsHidden == false);
            Assert.AreEqual(propertiesCount, grid.Columns.Count);

            IEnumerable<DisplayPropertyInfo> columnProperties = from p in displayProperties
                                                                where p.IsHidden == false
                                                                select p;

            int index = 0;
            foreach (var property in columnProperties)
            {
                ValidateColumnHeader(grid.Columns[index], property.ShortName, index);
                LogicalUiElementType elementType = ModelUiCreatorHelper.DetermineElementType(property);
                if (elementType == LogicalUiElementType.Date)
                {
                    ValidateColumntDate(grid.Columns[index], property);
                }
                else if (elementType == LogicalUiElementType.Options)
                {
                    ValidateColumntEnum(grid.Columns[index], property);
                }
                else if (elementType == LogicalUiElementType.Textbox)
                {
                    ValidateColumnTextBox(grid.Columns[index], property);
                }
                else
                {
                    Assert.Fail("Unhandled test case!");
                }
                index++;
            }
        }

        private void ValidateDependencyProperties(ModelDataGridUiInfo modelUiInfo, IList<DisplayPropertyInfo> properties)
        {
            var dependencyProperties = from p in properties
                                       where p.IsDependencyProperty == true
                                       select p;
            foreach (var property in dependencyProperties)
            {
                DataGridColumn column = modelUiInfo.Columns[property.DependencyHostPropertyName];
                ValidateDependencyProperty(column,
                                            property,
                                            ModelUiCreatorHelper.GetDataGridColumnDependencyProperty(property, column));
            }
        }

        private void ValidateDependencyProperty(DataGridColumn column, 
            DisplayPropertyInfo property, DependencyProperty dp)
        {
            ModellCreatorTestHelper.ValidateBinding(property, 
                BindingOperations.GetBinding(column, dp), 
                $"property = {property.PropertyName}");
        }
    }
}
