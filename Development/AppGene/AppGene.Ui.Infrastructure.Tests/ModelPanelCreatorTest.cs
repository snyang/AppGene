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
using AppGene.Common.Entities;
using System.Windows.Controls.Primitives;

namespace AppGene.Ui.Infrastructure.Tests
{
    [Apartment(ApartmentState.STA)]
    [TestFixture(typeof(EmployeeModel))]
    [TestFixture(typeof(DataTypeGroupA))]
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
            ValidationUiElements(creator, modelUiInfo, properties);

            ValidateDependencyProperties(modelUiInfo, properties);
        }

        private void ValidationUiElements(ModelPanelCreator creator, ModelUiInfo modelUiInfo, IList<DisplayPropertyInfo> properties)
        {
            Grid grid = modelUiInfo.Grid;
            int propertiesCount = properties.Count<DisplayPropertyInfo>(p => p.IsHidden == false);
            Assert.AreEqual(propertiesCount * 2, grid.Children.Count);

            var elementProperties = from p in properties
                                    where p.IsHidden == false
                                    select p;
            int index = 0;
            int row = 0;
            int column = 0;
            int rowsCount = (int)Math.Ceiling(((decimal)propertiesCount) / 2);
            foreach (var property in elementProperties)
            {
                string assertMessage = $"Property ({property.PropertyName})";
                ValidateUiElementLabel(grid.Children[index], property.Name + ":", row, column, assertMessage);

                LogicalUiElementType elementType = ModelUiCreatorHelper.DetermineElementType(property);
                if (elementType == LogicalUiElementType.Boolean)
                {
                    ValidateUiElementBoolean(grid.Children[index + 1], property.PropertyName, row, column + 1, assertMessage);
                }
                else if (elementType == LogicalUiElementType.ComboBox)
                {
                    ValidateUiElementComboBox(grid.Children[index + 1], property.PropertyName, row, column + 1, assertMessage);
                }
                else if (elementType == LogicalUiElementType.Date)
                {
                    ValidateUiElementDate(grid.Children[index + 1], property.PropertyName, row, column + 1, assertMessage);
                }
                else if (elementType == LogicalUiElementType.Options)
                {
                    ValidateUiElementEnum(grid.Children[index + 1], property.PropertyName, row, column + 1, assertMessage);
                }
                else if (elementType == LogicalUiElementType.Textbox)
                {
                    ValidateUiElementTextBox(grid.Children[index + 1], property.PropertyName, row, column + 1, assertMessage);
                }
                else
                {
                    Assert.Fail($"{assertMessage} : Unhandled test case, elementType = {elementType.ToString()}");
                }

                ModelUiCreatorHelper.CalculatePanelNextPosition(creator.Orientation, rowsCount, creator.ColumnsCount, ref column, ref row);
                index += 2;
            }
        }

        private void ValidateDependencyProperties(ModelUiInfo modelUiInfo, IList<DisplayPropertyInfo> properties)
        {
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

        private void ValidateUiElementDate(UIElement uiElement, string propertyName, int row, int column,
            string assertMessage)
        {
            Assert.IsTrue(uiElement is DatePicker, assertMessage);
            DatePicker datePicker = (DatePicker)uiElement;
            Assert.AreEqual(bindingPrefixPath + "/" + propertyName,
                datePicker.GetBindingExpression(DatePicker.SelectedDateProperty).ParentBinding.Path.Path,
                assertMessage);
            Assert.AreEqual(row, datePicker.GetValue(Grid.RowProperty),
                assertMessage);
            Assert.AreEqual(column, datePicker.GetValue(Grid.ColumnProperty),
                assertMessage);
        }

        private void ValidateUiElementEnum(UIElement uiElement, string propertyName, int row, int column,
            string assertMessage)
        {
            Assert.IsTrue(uiElement is WrapPanel, assertMessage);
            var enumPanel = (WrapPanel)uiElement;
            foreach (var item in enumPanel.Children)
            {
                Assert.IsTrue(item is RadioButton, assertMessage);
                var itemRadio = (RadioButton)item;
                Assert.AreEqual(bindingPrefixPath + "/" + propertyName,
                        itemRadio.GetBindingExpression(ToggleButton.IsCheckedProperty).ParentBinding.Path.Path,
                        assertMessage);
            }

            Assert.AreEqual(row, enumPanel.GetValue(Grid.RowProperty), assertMessage);
            Assert.AreEqual(column, enumPanel.GetValue(Grid.ColumnProperty), assertMessage);
        }

        private void ValidateUiElementLabel(UIElement uiElement, 
            string labelContent, 
            int row, 
            int column,
            string assertMessage)
        {
            Assert.IsTrue(uiElement is Label, assertMessage);
            Label label = (Label)uiElement;
            Assert.AreEqual(labelContent, label.Content, assertMessage);
            Assert.AreEqual(row, label.GetValue(Grid.RowProperty), assertMessage);
            Assert.AreEqual(column, label.GetValue(Grid.ColumnProperty), assertMessage);
        }

        private void ValidateUiElementBoolean(UIElement uiElement, string propertyName, int row, int column,
            string assertMessage)
        {
            ValidateUiElement<CheckBox>(uiElement, ToggleButton.IsCheckedProperty, propertyName, row, column, assertMessage);
        }

        private void ValidateUiElementComboBox(UIElement uiElement, string propertyName, int row, int column,
            string assertMessage)
        {
            ValidateUiElement<ComboBox>(uiElement, Selector.SelectedItemProperty, propertyName, row, column, assertMessage);
        }

        private void ValidateUiElementTextBox(UIElement uiElement, string propertyName, int row, int column,
            string assertMessage)
        {
            ValidateUiElement<TextBox>(uiElement, TextBox.TextProperty, propertyName, row, column, assertMessage);
        }

        private void ValidateUiElement<T>(UIElement uiElement, 
            DependencyProperty bindingProperty,
            string propertyName, 
            int row, 
            int column,
            string assertMessage)
            where T : FrameworkElement
        {
            Assert.IsTrue(uiElement is T, assertMessage);
            var element = (T)uiElement;
            Assert.AreEqual(bindingPrefixPath + "/" + propertyName,
                element.GetBindingExpression(bindingProperty).ParentBinding.Path.Path,
                assertMessage);
            Assert.AreEqual(row, element.GetValue(Grid.RowProperty), assertMessage);
            Assert.AreEqual(column, element.GetValue(Grid.ColumnProperty), assertMessage);
        }
    }
}
