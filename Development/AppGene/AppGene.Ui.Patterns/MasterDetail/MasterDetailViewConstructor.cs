using AppGene.Ui.Infrastructure;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace AppGene.Ui.Patterns.MasterDetail
{
    public class MasterDetailViewConstructor<TModel, TEntity>
        where TModel : class, new()
        where TEntity : class, new()
    {
        #region Constants fields

        private const string FrameworkElementErrorStyle = "FrameworkElementErrorStyle";

        #endregion Constants fields

        public MasterDetailViewConstructor(MasterDetailPatternContext<TModel, TEntity> patternContext, ContentControl owner)
        {
            PatternContext = patternContext;
            Owner = owner;
        }

        #region Properties

        private Button buttonCancel;
        private Button buttonDelete;
        private Button buttonNew;
        private Button buttonOk;
        private Button buttonRefresh;
        public DataGrid DataGridMain { get; private set; }
        public MasterDetailPatternContext<TModel, TEntity> PatternContext { get; set; }
        private Grid GridContainer { get; set; }
        private Grid GridDetail { get; set; }
        private ContentControl Owner { get; set; }

        #endregion Properties

        public void Initialize()
        {
            InitContainer();
            InitCommandBar();
            InitDataGrid();
            InitDetailPanel();
            initController();
        }

        private static Style GetResourceStyle(string resourceName)
        {
            return Application.Current.Resources[resourceName] as Style;
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

            // attach to UI
            this.GridContainer.Children.Add(tray);
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

        private void initController()
        {
            var Controller = PatternContext.ViewController;

            Controller.Owner = this.Owner;
            Controller.DataGridMain = this.DataGridMain;
            Controller.ButtonCancel = this.buttonCancel;
            Controller.ButtonOk = this.buttonOk;
            Controller.ButtonDelete = this.buttonDelete;
            Controller.ButtonNew = this.buttonNew;
            Controller.ButtonRefresh = this.buttonRefresh;

            Controller.Initialize();
        }

        private void InitDataGrid()
        {
            var editProperties = PatternContext.UiService.GetGridDisplayProperties();
            Style style = GetResourceStyle(FrameworkElementErrorStyle);
            DataGridMain = new ModelDataGridCreator().Create(editProperties, style);
            this.GridContainer.Children.Add(DataGridMain);
        }

        private void InitDetailPanel()
        {
            // get edit properties
            var editProperties = PatternContext.UiService.GetDisplayProperties();
            Style style = GetResourceStyle(FrameworkElementErrorStyle);
            string bindingPathPrefix = "CollectionView";

            // create detail panel.
            GridDetail = new ModelPanelCreator().Create(editProperties, 2, style, bindingPathPrefix).Grid;

            // add to parent
            this.GridContainer.Children.Add(GridDetail);

            // TODO: move it to a new layout manager class
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
    }
}