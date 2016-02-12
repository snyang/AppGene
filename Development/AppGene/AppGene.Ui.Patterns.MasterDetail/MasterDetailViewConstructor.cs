﻿using AppGene.Business.Infrastructure;
using AppGene.Model.EntityPerception;
using AppGene.Ui.Infrastructure;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace AppGene.Ui.Patterns.MasterDetail
{
    public class MasterDetailViewConstructor<TEntity, TModel, TDataService>
        where TEntity : class, new()
        where TModel : IMasterDetailModel<TEntity>, new()
        where TDataService : AbstractCrudBusinessService<TEntity>, new()
    {
        #region Constants fields

        private const string FrameworkElementErrorStyle = "FrameworkElementErrorStyle";

        #endregion

        public MasterDetailViewConstructor(ContentControl owner)
        {
            Owner = owner;
        }

        #region Properties

        private Grid GridDetail { get; set; }

        private Grid GridContainer { get; set; }

        public DataGrid DataGridMain { get; private set; }

        private ContentControl Owner { get; set; }

        private MasterDetailController<TEntity, TModel, TDataService> Controller { get; set; }

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
            Controller = new MasterDetailController<TEntity, TModel, TDataService>()
            {
                Owner = this.Owner,
                DataGridMain = this.DataGridMain,
                ButtonCancel = this.buttonCancel,
                ButtonOk = this.buttonOk,
                ButtonDelete = this.buttonDelete,
                ButtonNew = this.buttonNew,
                ButtonRefresh = this.buttonRefresh
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

            // attach to UI
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

                //TODO: distinct edit properties for grid and detail panel
                // create data grid columns.
                var editProperties = new EditPropertiesGetter().Get(new EntityAnalysisContext
                {
                    EntityType = typeof(TEntity),
                    Source = this.GetType().FullName,
                });

                Style style = GetResourceStyle(FrameworkElementErrorStyle);
                foreach (var property in editProperties)
                {
                    DataGridColumn column = PropertyDataGridColumnCreator.Create(property, style);
                    DataGridMain.Columns.Add(column);
                }
            }
            finally
            {
                DataGridMain.EndInit();
            }
        }



        private void InitDetailPanel()
        {
            GridDetail = new Grid
            {
                Margin = new Thickness(4)
            };
            Grid.SetRow(GridDetail, 2);

            // get edit properties
            var editProperties = new EditPropertiesGetter().Get(new EntityAnalysisContext
            {
                EntityType = typeof(TEntity),
                Source = this.GetType().FullName,
            });

            // create columns
            UiTool.CreateGridColumns(GridDetail, 4);

            // create rows
            UiTool.CreateGridRows(GridDetail, (int)Math.Ceiling((decimal)(editProperties.Count / 2)));

            // Create Fields
            Style style = GetResourceStyle(FrameworkElementErrorStyle);
            int row = 0;
            int column = 0;
            foreach (var property in editProperties)
            {
                PropertyControlCreator.Create(property,
                    GridDetail,
                    row,
                    column,
                    "CollectionView",
                    style);

                if (column == 2)
                {
                    row++;
                    column = 0;
                }
                else
                {
                    column += 2;
                }
            }

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