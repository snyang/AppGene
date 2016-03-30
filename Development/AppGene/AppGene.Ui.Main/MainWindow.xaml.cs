using AppGene.Business.Sample;
using AppGene.Common.Entities;
using AppGene.Common.Entities.Models;
using AppGene.Ui.Patterns.MasterDetail;
using System.Windows;

namespace AppGene.Ui.Main
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void employeeButton_Click(object sender, RoutedEventArgs e)
        {
            var patterContext = new MasterDetailPatternContext<EmployeeModel, Employee>(new CommonCrudBusinessService<Employee>());
            var window = new MasterDetailView<EmployeeModel, Employee>(patterContext);
            window.ShowDialog();
        }

        private void dataTypeGroupAButton_Click(object sender, RoutedEventArgs e)
        {
            var patterContext = new MasterDetailPatternContext<DataTypeGroupA, DataTypeGroupA>(new CommonCrudBusinessService<DataTypeGroupA>());
            var window = new MasterDetailView<DataTypeGroupA, DataTypeGroupA>(patterContext);
            window.ShowDialog();
        }
    }
}