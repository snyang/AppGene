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

namespace AppGene.Ui.Infrastructure.Tests
{
    [Apartment(ApartmentState.STA)]
    [TestFixture(typeof(EmployeeModel))]
    public class ModelPanelCreatorTest<TModel>
    {
        private string bindingPrefixPath = "CollectionView";
        [Test]
        public void TestCreateGrid()
        {
            var properties = new DisplayPropertiesGetter().GetProperties(new EntityAnalysisContext()
            {
                EntityType = typeof(TModel)
            });
            var creator = new ModelPanelCreator();
            var modelUiInfo = creator.Create(properties, 2, null, bindingPrefixPath);

            ValidateCreateGrid(creator, modelUiInfo, properties);
        }

        private void ValidateCreateGrid(ModelPanelCreator creator, ModelUiInfo modelUiInfo, IList<DisplayPropertyInfo> properties)
        {
            Grid grid = modelUiInfo.Grid;
            int propertiesCount = properties.Count<DisplayPropertyInfo>(p => p.IsHidden == false);
            Assert.AreEqual(propertiesCount * 2, grid.Children.Count);

            var columnProperties = from p in properties
                                   where p.IsHidden == false
                                   select p;

            int index = 0;
            int row = 0;
            int column = 0;
            int rowsCount = (int)Math.Ceiling(((decimal)propertiesCount) / 2);
            foreach (var property in columnProperties)
            {
                ValidateUiElementLabel(grid.Children[index], property.Name + ":", row, column);

                LogicalUiElementType elementType = ModelUiCreatorHelper.DetermineElementType(property);
                if (elementType == LogicalUiElementType.Date)
                {
                    ValidateUiElementDate(grid.Children[index + 1], property.PropertyName, row, column + 1);
                }
                else if (elementType == LogicalUiElementType.Options)
                {
                    // TODO validate enum
                }
                else if (elementType == LogicalUiElementType.Textbox)
                {
                    ValidateUiElementTextBox(grid.Children[index + 1], property.PropertyName, row, column + 1);
                }
                else
                {
                    Assert.Fail("Unhandled test case! ");
                }

                ModelUiCreatorHelper.CalculatePanelNextPosition(creator.Orientation, rowsCount, creator.ColumnsCount, ref column, ref row);
                index += 2;
            }

            var dependencyProperties = from p in properties
                                       where p.IsDependencyProperty == true
                                       select p;
            foreach (var property in dependencyProperties)
            {
                ModelPropertyUiInfo propertyUiInfo = modelUiInfo.PropertyUiInfos[property.DependencyHostPropertyName];
                ValidateDependencyProperty(propertyUiInfo.Content as FrameworkElement,
                                            property.PropertyName,
                                            ModelUiCreatorHelper.GetDependencyProperty(property, propertyUiInfo.Content));
            }
        }

        private void ValidateDependencyProperty(FrameworkElement uiElement, string propertyName, DependencyProperty dp)
        {
            Assert.AreEqual(bindingPrefixPath + "/" + propertyName,
                uiElement.GetBindingExpression(dp).ParentBinding.Path.Path);
        }

        private void ValidateUiElementDate(UIElement uiElement, string propertyName, int row, int column)
        {
            Assert.IsTrue(uiElement is DatePicker);
            DatePicker datePicker = (DatePicker)uiElement;
            Assert.AreEqual(bindingPrefixPath + "/" + propertyName,
                datePicker.GetBindingExpression(DatePicker.SelectedDateProperty).ParentBinding.Path.Path);
            Assert.AreEqual(row, datePicker.GetValue(Grid.RowProperty));
            Assert.AreEqual(column, datePicker.GetValue(Grid.ColumnProperty));
        }

        private void ValidateUiElementLabel(UIElement uiElement, string labelContent, int row, int column)
        {
            Assert.IsTrue(uiElement is Label);
            Label label = (Label)uiElement;
            Assert.AreEqual(labelContent, label.Content);
            Assert.AreEqual(row, label.GetValue(Grid.RowProperty));
            Assert.AreEqual(column, label.GetValue(Grid.ColumnProperty));
        }

        private void ValidateUiElementTextBox(UIElement uiElement, string propertyName, int row, int column)
        {
            Assert.IsTrue(uiElement is TextBox);
            TextBox textBox = (TextBox)uiElement;
            Assert.AreEqual(bindingPrefixPath + "/" + propertyName, textBox.GetBindingExpression(TextBox.TextProperty).ParentBinding.Path.Path);
            Assert.AreEqual(row, textBox.GetValue(Grid.RowProperty));
            Assert.AreEqual(column, textBox.GetValue(Grid.ColumnProperty));
        }
    }
}
