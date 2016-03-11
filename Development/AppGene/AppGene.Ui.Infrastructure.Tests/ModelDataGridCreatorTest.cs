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

namespace AppGene.Ui.Infrastructure.Tests
{
    [Apartment(ApartmentState.STA)]
    [TestFixture(typeof(EmployeeModel))]
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
            DataGrid dataGrid = creator.Create(displayProperties, null);

            ValidateCreateGrid(dataGrid, displayProperties);
        }

        private void ValidateCreateGrid(DataGrid grid, IList<DisplayPropertyInfo> displayProperties)
        {

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
                    // TODO validate enum
                }
                else if (elementType == LogicalUiElementType.Textbox)
                {
                    ValidateColumnTextBox(grid.Columns[index], property);
                }
                else
                {
                    Assert.Fail("Unhandled test case! ");
                }
                index++;
            }
            
            //ValidateDependencyProperty(grid.Columns[index],
            //    "FemaleBenefit_IsReadOnly",
            //    TextBox.IsReadOnlyCaretVisibleProperty);
        }

        private void ValidateDependencyProperty(DataGridColumn gridColumn, string propertyName, DependencyProperty dp)
        {
            //TODO: Assert.AreEqual(propertyName, gridColumn.GetBindingExpression(dp).ParentBinding.Path.Path);
        }

        private void ValidateColumntDate(DataGridColumn gridColumn, DisplayPropertyInfo property)
        {
            Assert.IsTrue(gridColumn is DataGridTemplateColumn);
            DataGridTemplateColumn textColumn = (DataGridTemplateColumn)gridColumn;

            // TODO Assert.AreEqual(propertyName, (textColumn.CellTemplate.VisualTree.Binding as Binding).Path);
        }

        private void ValidateColumnHeader(DataGridColumn gridColumn, string header, int column)
        {
            Assert.AreEqual(header, gridColumn.Header);
        }

        private void ValidateColumnTextBox(DataGridColumn gridColumn, DisplayPropertyInfo property)
        {
            Assert.IsTrue(gridColumn is DataGridTextColumn);
            DataGridTextColumn textColumn = (DataGridTextColumn)gridColumn;
            
            Assert.AreEqual(property.PropertyName, (textColumn.Binding as Binding).Path.Path);
            Assert.AreEqual(property.IsReadOnly
                    ? BindingMode.OneWay
                    : BindingMode.TwoWay,
                    (textColumn.Binding as Binding).Mode);
        }
    }
}
