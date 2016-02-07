using AppGene.Business.Sample;
using AppGene.Db.Core;
using AppGene.Model.Entities;
using AppGene.Ui;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AppGene
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [Export]
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Sets the ViewModel.
        /// </summary>
        /// <remarks>
        /// This set-only property is annotated with the <see cref="ImportAttribute"/> so it is injected by MEF with
        /// the appropriate view model.
        /// </remarks>
        [Import]
        [SuppressMessage("Microsoft.Design", "CA1044:PropertiesShouldNotBeWriteOnly", Justification = "Needs to be a property to be composed by MEF")]
        ShellViewModel ViewModel
        {
            set
            {
                this.DataContext = value;
            }
        }

        private void employeeButton_Click(object sender, RoutedEventArgs e)
        {
            BaseUiModel<Employee> test = new BaseUiModel<Employee>(null);
            
            //test.ValidateColumn
            var employee = new MasterDetailWindow<Employee, EmployeeService>();
            employee.ShowDialog();
        }
    }
}
