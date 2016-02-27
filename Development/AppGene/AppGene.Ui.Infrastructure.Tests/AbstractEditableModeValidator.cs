using System;
using AppGene.Ui.Infrastructure.Mvvm;
using AppGene.Common.Entities;
using System.Windows.Data;
using System.Windows.Controls;
using System.ComponentModel;
using NUnit.Framework;
using System.Threading;
using AppGene.Ui.Infrastructure.Tests.TestData;
using AppGene.Common.Entities.Infrastructure.EntityModels;

namespace AppGene.Ui.Infrastructure.Tests
{
    [Apartment(ApartmentState.STA)]
    [TestFixture(typeof(Employee))]
    [TestFixture(typeof(EmployeeModel))]
    public class AbstractEditableModelValidator<TModel>
        where TModel : class, new()
    {
        private DefaultEditableModel<TModel, Employee> Model { get; set; }
        private DatePicker elementBirthday { get; set; }
        private TextBox elementEmployeeName { get; set; }
        private DateTime InitBirthday { get; set; } = new DateTime(2000, 1, 1);
        private string InitEmployeeName { get; set; } = "Test User 1";

        [SetUp]
        public void Init()
        {
            var entity = new Employee()
            {
                EmployeeId = 1,
                EmployeeCode = 1000000,
                EmployeeName = InitEmployeeName,
                Birthday = InitBirthday,
                Gender = Gender.Male
            };

            Model = new DefaultEditableModel<TModel, Employee>(entity);
            elementEmployeeName = CreateUiElementTextbox("EmployeeName", false);
            elementBirthday = CreateUiElementForDatePicker("Birthday");
        }

        [TearDown]
        public void Cleanup()
        { /* ... */ }

        [Test]
        public void TestDatePickerDataBinding()
        {

            Assert.AreEqual(InitBirthday, elementBirthday.SelectedDate);

            DateTime newBirthday = new DateTime(2000, 1, 2);
            elementBirthday.SelectedDate = newBirthday;

            Assert.AreEqual(newBirthday, Model.Entity.Birthday);
        }

        [Test]
        public void TestEditable()
        {
            Model.BeginEdit();
            string originalEmployeeName = Model.Entity.EmployeeName;
            string newEmployeeName = "editable";
            elementEmployeeName.Text = newEmployeeName;
            Assert.IsTrue(Model.ToIEditableModel().IsChanged);

            Model.CancelEdit();
            Assert.AreEqual(originalEmployeeName, Model.Entity.EmployeeName);
            Assert.IsFalse(Model.ToIEditableModel().IsChanged);

            Model.BeginEdit();
            elementEmployeeName.Text = newEmployeeName;
            Assert.IsTrue(Model.ToIEditableModel().IsChanged);

            Model.EndEdit();
            Assert.AreEqual(newEmployeeName, Model.Entity.EmployeeName);
            Assert.IsFalse(Model.ToIEditableModel().IsChanged);
        }

        [Test]
        public void TestGetEntity()
        {
            string newEmployeeName = "test get entity";
            elementEmployeeName.Text = newEmployeeName;
            Assert.AreEqual(newEmployeeName, Model.Entity.EmployeeName);
        }

        [Test]
        public void TestTextBoxDataBinding()
        {

            Assert.AreEqual(InitEmployeeName, elementEmployeeName.Text);

            string newEmployeeName = "test user 2";
            elementEmployeeName.Text = newEmployeeName;

            Assert.AreEqual(newEmployeeName, Model.Entity.EmployeeName);
        }

        [Test]
        public void TestValidation()
        {
            string newEmployeeName = "";
            elementEmployeeName.Text = newEmployeeName;
            String validateErrors = ((IDataErrorInfo)Model).Error;
            Assert.IsFalse(string.IsNullOrEmpty(validateErrors));
        }

        internal int CalculateAge(DateTime birthday)
        {
            int age = DateTime.Today.Year - birthday.Year;
            return age > 0 ? age : 0;
        }

        [Test]
        public void TestComputedProperty()
        {
            if (EntityModelHelper.AreEntityAndModelSame<TModel, Employee>()) return;

            TextBox TextBoxAge = CreateUiElementTextbox("Age", true);

            Assert.AreEqual(CalculateAge(Model.Entity.Birthday), Convert.ToInt32(TextBoxAge.Text));

            elementBirthday.SelectedDate = new DateTime(2001, 1, 1);
            Assert.AreEqual(CalculateAge(Model.Entity.Birthday), Convert.ToInt32(TextBoxAge.Text));
        }

        private DatePicker CreateUiElementForDatePicker(string name)
        {
            DatePicker control = new DatePicker
            {
                Name = "datePicker" + name
            };
            control.DataContext = Model;
            control.SetBinding(DatePicker.SelectedDateProperty, new Binding(name)
            {
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                Mode = BindingMode.TwoWay,
                ValidatesOnDataErrors = true,
                NotifyOnValidationError = true,
                ValidatesOnExceptions = true,
            });

            return control;
        }

        private TextBox CreateUiElementTextbox(string name, bool isReadOnly)
        {
            TextBox control = new TextBox
            {
                Name = "textBox" + name,
            };
            control.DataContext = Model;
            if (isReadOnly)
            {
                control.SetBinding(TextBox.TextProperty, new Binding(name)
                {
                    UpdateSourceTrigger = UpdateSourceTrigger.Explicit,
                    Mode = BindingMode.OneWay,
                });
            }
            else
            { 
                control.SetBinding(TextBox.TextProperty, new Binding(name)
                {
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                    Mode = BindingMode.TwoWay,
                    ValidatesOnDataErrors = true,
                    NotifyOnValidationError = true,
                    ValidatesOnExceptions = true,
                });
            }

            return control;
        }
    }
}
