using AppGene.Business.Sample;
using AppGene.Common.Entities;
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
            var patterContext = new MasterDetailPatternContext<Employee, Employee>(new CommonCrudBusinessService<Employee>());
            var employee = new MasterDetailView<Employee, Employee>(patterContext);
            employee.ShowDialog();
        }
    }
}