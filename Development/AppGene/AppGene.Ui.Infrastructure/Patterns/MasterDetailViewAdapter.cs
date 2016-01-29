using AppGene.Db.Core;
using AppGene.Modules.Employee.Ui.Services;
using AppGene.Ui.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace AppGene.Ui.Infrastructure.Patterns
{
    public class MasterDetailViewAdapter<TModel, TDataService>
        where TModel : IUiModel, new()
        where TDataService : IDataService<TModel>, new()
    {
        #region Constants fields

        private const string FrameworkElementErrorStyle = "FrameworkElementErrorStyle";

        #endregion

        public MasterDetailViewAdapter(ContentControl owner)
        {
            Owner = owner;
        }

        #region Properties

        private Grid GridDetail { get; set; }

        private Grid GridContainer { get; set; }

        public DataGrid DataGridMain { get; private set; }

        private ContentControl Owner { get; set; }

        private MasterDetailController<TModel, TDataService> Controller { get; set; }

        private Button buttonDelete;
        private Button buttonRefresh;
        private Button buttonNew;
        private Button buttonOk;
        private Button buttonCancel;

        #endregion

        public void Initialize()
        {
            InitContainer();
            InitCommandBar();
            InitDataGrid();
            InitDetailPanel();
            initController();
        }

        private void initController()
        {
            Controller = new MasterDetailController<TModel, TDataService>()
            {
                Owner = this.Owner,
                DataGridMain = this.DataGridMain,
                ButtonCancel = this.buttonCancel,
                ButtonOk = this.buttonOk,
                ButtonDelete = this.buttonDelete,
                ButtonNew = this.buttonNew,
                ButtonRefresh =  this.buttonRefresh
            };
            Controller.Initialize();
        }

        private void InitContainer()
        {
            this.GridContainer = new Grid();

            // toolbar row
            GridContainer.RowDefinitions.Add(new RowDefinition
            {
                Height = GridLength.Auto
            });

            // data grid row
            GridContainer.RowDefinitions.Add(new RowDefinition
            {
                Height = new GridLength(1, GridUnitType.Star)
            });

            // detail panel row
            GridContainer.RowDefinitions.Add(new RowDefinition
            {
                Height = GridLength.Auto
            });

            // detail command row
            GridContainer.RowDefinitions.Add(new RowDefinition
            {
                Height = GridLength.Auto
            });

            Owner.Content = GridContainer;
        }

        private void InitCommandBar()
        {
            ToolBarTray tray = new ToolBarTray();
            Grid.SetRow(tray, 0);

            // Create a toolbar
            ToolBar toolbar = new ToolBar();
            tray.ToolBars.Add(toolbar);

            // Create new command
            buttonNew = new Button
            {
                Content = "New"
            };
            toolbar.Items.Add(buttonNew);

            // Create delete command
            buttonDelete = new Button
            {
                Content = "Delete"
            };
            buttonDelete.CommandParameter = this.DataGridMain;
            toolbar.Items.Add(buttonDelete);

            // Create refresh command
            buttonRefresh = new Button
            {
                Content = "Refresh"
            };
            toolbar.Items.Add(buttonRefresh);

            toolbar.Items.Add(new Separator());

            toolbar.Items.Add(new Button
            {
                Content = "Cut",
                Command = ApplicationCommands.Cut
            });

            toolbar.Items.Add(new Button
            {
                Content = "Copy",
                Command = ApplicationCommands.Copy
            });

            toolbar.Items.Add(new Button
            {
                Content = "Paste",
                Command = ApplicationCommands.Paste
            });

            toolbar.Items.Add(new Separator());

            toolbar.Items.Add(new Label
            {
                Content = "Filter:",
            });

            TextBox textBoxFilter = new TextBox
            {
                Width = 200,
                ToolTip = "Filter",
            };

            textBoxFilter.SetBinding(TextBox.TextProperty, new Binding("FilterString")
            {
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                Mode = BindingMode.TwoWay
            });

            toolbar.Items.Add(textBoxFilter);

            // attch to UI
            this.GridContainer.Children.Add(tray);
        }

        private void InitDataGrid()
        {
            try
            {
                DataGridMain = new DataGrid
                {
                    Margin = new Thickness(2)
                };
                this.GridContainer.Children.Add(DataGridMain);
                Grid.SetRow(DataGridMain, 1);

                DataGridMain.BeginInit();
                DataGridMain.AutoGenerateColumns = false;
                DataGridMain.IsSynchronizedWithCurrentItem = true;
                DataGridMain.Columns.Clear();

                //dataGrid.Columns.
                DataGridMain.Columns.Add(new DataGridTextColumn
                {
                    Header = "Code",
                    EditingElementStyle = GetResourceStyle(FrameworkElementErrorStyle),
                    ElementStyle = GetResourceStyle(FrameworkElementErrorStyle),
                    Binding = new Binding("EmployeeCode")
                    {
                        UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                        Mode = BindingMode.TwoWay,
                        ValidatesOnDataErrors = true,
                        NotifyOnValidationError = true,
                        ValidatesOnExceptions = true,
                        StringFormat = "0000"
                    }
                });

                DataGridMain.Columns.Add(new DataGridTextColumn
                {
                    Header = "Name",
                    EditingElementStyle = GetResourceStyle(FrameworkElementErrorStyle),
                    ElementStyle = GetResourceStyle(FrameworkElementErrorStyle),
                    Binding = new Binding("EmployeeName")
                    {
                        UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                        Mode = BindingMode.TwoWay,
                        ValidatesOnDataErrors = true,
                        NotifyOnValidationError = true,
                        ValidatesOnExceptions = true,
                    }
                });

                // Gender
                // https://msdn.microsoft.com/zh-cn/library/system.windows.controls.datagridcomboboxcolumn.aspx
                ObjectDataProvider provider = new ObjectDataProvider()
                {
                    ObjectType = typeof(Enum),
                    MethodName = "GetValues",

                };
                provider.MethodParameters.Add(typeof(Db.Model.Genders));

                DataGridComboBoxColumn genderColumn = new DataGridComboBoxColumn
                {
                    Header = "Gender",
                    EditingElementStyle = GetResourceStyle(FrameworkElementErrorStyle),
                    ElementStyle = GetResourceStyle(FrameworkElementErrorStyle),
                    SelectedItemBinding = new Binding("Gender")
                    {
                        UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                        Mode = BindingMode.TwoWay,
                        ValidatesOnDataErrors = true,
                        NotifyOnValidationError = true,
                        ValidatesOnExceptions = true,

                    },
                };

                //http://stackoverflow.com/questions/25644003/wpf-datagridcomboboxcolumn-binding
                BindingOperations.SetBinding(genderColumn, DataGridComboBoxColumn.ItemsSourceProperty, new Binding()
                {
                    Source = provider
                });
                DataGridMain.Columns.Add(genderColumn);

                // Column Birthday Binding
                Binding birthdayBinding = new Binding("Birthday")
                {
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                    Mode = BindingMode.TwoWay,
                    ValidatesOnDataErrors = true,
                    NotifyOnValidationError = true,
                    ValidatesOnExceptions = true,
                    StringFormat = "d"
                };

                // Column Birthday
                DataGridMain.Columns.Add(new DataGridTemplateColumn
                {
                    Header = "Birthday",
                    CellTemplate = CreateDataTemplate<TextBlock>(TextBlock.TextProperty, birthdayBinding),
                    CellEditingTemplate = CreateDataTemplate<DatePicker>(DatePicker.TextProperty, birthdayBinding),
                    CellStyle = GetResourceStyle(FrameworkElementErrorStyle),
                });
            }
            finally
            {
                DataGridMain.EndInit();
            }
        }

        private static DataTemplate CreateDataTemplate<T>(DependencyProperty bindingProperty, Binding binding)
        {
            FrameworkElementFactory cellElement = new FrameworkElementFactory(typeof(T));
            cellElement.SetBinding(bindingProperty, binding);

            // Column Birthday CellTemplate
            DataTemplate cellTemplate = new DataTemplate(typeof(T))
            {
                VisualTree = cellElement,
            };

            return cellTemplate;
        }

        private void InitDetailPanel()
        {
            GridDetail = new Grid
            {
                Margin = new Thickness(4)
            };
            Grid.SetRow(GridDetail, 2);

            // create rows
            UiTool.CreateGridRows(GridDetail, 2);

            // create columns
            UiTool.CreateGridColumns(GridDetail, 4);

            // Create Fields
            UiTool.CreateField(GridDetail, "EmployeeCode", "Employee Code", "CollectionView/EmployeeCode", GetResourceStyle(FrameworkElementErrorStyle), 0, 0);
            UiTool.CreateField(GridDetail, "EmployeeName", "Employee Name", "CollectionView/EmployeeName", GetResourceStyle(FrameworkElementErrorStyle), 0, 2);
            UiTool.CreateEnumField(GridDetail, typeof(Db.Model.Genders), "Gender", "Gender", "CollectionView/Gender", GetResourceStyle(FrameworkElementErrorStyle), 1, 0);
            UiTool.CreateDateField(GridDetail, "Birthday", "Birthday", "CollectionView/Birthday", null, 1, 2);

            // add to parent
            this.GridContainer.Children.Add(GridDetail);

            // For details commands
            Grid gridDetailsCommand = new Grid
            {
                Margin = new Thickness(4)
            };
            Grid.SetRow(gridDetailsCommand, 3);

            // create columns
            UiTool.CreateCommandGridColumns(gridDetailsCommand, 3);

            // create commands
            buttonOk = UiTool.CreateGridCommand(gridDetailsCommand, "buttonOk", "OK", 1);
            buttonCancel = UiTool.CreateGridCommand(gridDetailsCommand, "buttonCancel", "Cancel", 2);
            
            // add to parent
            this.GridContainer.Children.Add(gridDetailsCommand);
        }

        private Style GetResourceStyle(string resourceName)
        {
            return Application.Current.Resources[FrameworkElementErrorStyle] as Style;
        }
    }
}
