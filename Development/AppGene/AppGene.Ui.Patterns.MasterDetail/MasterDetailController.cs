using AppGene.Ui.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace AppGene.Ui.Patterns.MasterDetail
{
    /// <summary>
    /// The class is used to as a controller to control MasterDetailView operations
    /// </summary>
    public class MasterDetailController<TEntity, TModel>
        where TEntity : class, new()
        where TModel : IMasterDetailModel<TEntity>, new()
    {
        private int currentIndex;
        private readonly DelegateCommand deleteCommand;
        private readonly DelegateCommand newCommand;

        public MasterDetailController()
        {
            newCommand = new DelegateCommand(this.DoNew, this.CanExecuteNew);
            deleteCommand = new DelegateCommand(this.DoDelete, this.CanExecuteDelete);
        }

        #region Properties

        public ContentControl Owner { get; set; }
        public DataGrid DataGridMain { get; set; }
        public Button ButtonCancel { get; set; }
        public Button ButtonDelete { get; set; }
        public Button ButtonNew { get; set; }
        public Button ButtonOk { get; set; }
        public Button ButtonRefresh { get; set; }

        #endregion Properties

        public MasterDetailViewModel<TEntity, TModel> ViewModel
        {
            get
            {
                return this.Owner.DataContext as MasterDetailViewModel<TEntity, TModel>;
            }
            set
            {
                this.Owner.DataContext = value;
            }
        }

        public void Initialize()
        {
            initOwner();
            HandleEvent();
            BindingData();
            BindingCommands();
        }

        private void BindingData()
        {
            DataGridMain.SetBinding(DataGrid.ItemsSourceProperty,
                new Binding("CollectionView")
                {
                    Mode = BindingMode.TwoWay
                });
            DataGridMain.SetBinding(DataGrid.SelectedItemProperty,
                new Binding("CurrentItem")
                {
                    Mode = BindingMode.TwoWay,
                });
        }

        private void BindingCommands()
        {
            ButtonNew.Command = newCommand;
            ButtonDelete.Command = deleteCommand;
            ButtonRefresh.Command = new DelegateCommand(this.DoRefresh);
            ButtonOk.Command = new DelegateCommand(this.DoOk);
            ButtonCancel.Command = new DelegateCommand(this.DoCancel);
        }

        private void HandleEvent()
        {
            DataGridMain.SelectionChanged += DataGrid_SelectionChanged;
            DataGridMain.AddingNewItem += DataGrid_AddingNewItem;
            CommandManager.AddPreviewCanExecuteHandler(DataGridMain, DataGrid_PreviewCanExecuteHandler);
        }

        private void initOwner()
        {
            Owner.Loaded += Owner_Loaded;
        }

        private void Owner_Loaded(object sender, RoutedEventArgs e)
        {
            UiTool.HandleUiEvent(() =>
                {
                    GetData();
                });
        }

        public void GetData()
        {
            ViewModel.GetData();
            ViewModel.CollectionView.CurrentChanged += CollectionView_CurrentChanged;
        }

        private void CollectionView_CurrentChanged(object sender, EventArgs e)
        {
            newCommand.OnCanExecuteChanged();
            deleteCommand.OnCanExecuteChanged();
            if (ViewModel.CurrentItem == null) return;
            TModel item = (TModel)ViewModel.CurrentItem;
            if (item == null) return;
            ((IEditableObject)item).BeginEdit();
        }

        public void DoCancel()
        {
            if (ViewModel.CurrentItem == null) return;
            TModel item = (TModel)ViewModel.CurrentItem;
            if (item == null) return;

            if (ViewModel.IsAddingNew)
            {
                ViewModel.CollectionView.Remove(item);
            }
            else
            {
                ((IEditableObject)item).CancelEdit();
                ((IEditableObject)item).BeginEdit();
            }
        }

        public void DoOk()
        {
            UiTool.HandleUiEvent(() =>
            {
                if (ViewModel.CurrentItem == null) return;
                TModel item = (TModel)ViewModel.CurrentItem;
                ViewModel.DataSave(item);
                ((IEditableObject)item).BeginEdit();
            });
        }

        private void DoRefresh()
        {
            UiTool.HandleUiEvent(() =>
                {
                    GetData();
                });
        }

        private bool CanExecuteNew()
        {
            return !ViewModel.IsChanged;
        }

        private void DoNew()
        {
            TModel item = (TModel)ViewModel.CollectionView.AddNew();
            SetNewItem(item);
            ViewModel.CollectionView.CommitNew();
        }

        private bool CanExecuteDelete()
        {
            if (DataGridMain.SelectedItems.Count == 0)
            {
                return false;
            }

            return true;
        }

        private void DoDelete()
        {
            UiTool.HandleUiEvent(() =>
            {
                DeleteItems();
            });
        }

        private void DeleteItems()
        {
            if (DataGridMain.SelectedItems.Count == 0)
            {
                return;
            }

            int selectedIndex = DataGridMain.SelectedIndex;
            IList<TModel> models = ViewModel.Delete(DataGridMain.SelectedItems);
            foreach (var item in models)
            {
                ViewModel.CollectionView.Remove(item);
            }

            if (selectedIndex < DataGridMain.Items.Count)
            {
                DataGridMain.SelectedIndex = selectedIndex;
            }
            else
            {
                DataGridMain.SelectedIndex = DataGridMain.Items.Count - 1;
            }
        }

        private void DataGrid_PreviewCanExecuteHandler(object sender, CanExecuteRoutedEventArgs e)
        {
            //https://social.msdn.microsoft.com/Forums/vstudio/en-US/04cb454b-fd58-4b6d-8c67-550657dda8a0/datagrid-delete-row-handling-with-delete-keydown-event-problem?forum=wpf
            DataGrid grid = (DataGrid)sender;
            if (e.Command == DataGrid.DeleteCommand)
            {
                e.Handled = true;
                string confirmDeleteMessage = grid.SelectedItems.Count > 1
                    ? "Would you like to delete selected items?"
                    : String.Format(CultureInfo.CurrentCulture,
                        "Would you like to delete '{0}'?", 
                        (grid.SelectedItem as IMasterDetailModel<TEntity>).ToDisplayString());

                if (MessageBox.Show(confirmDeleteMessage,
                    "Confirm Delete",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question,
                    MessageBoxResult.No) != MessageBoxResult.Yes)
                {
                    return;
                }

                UiTool.HandleUiEvent(() =>
                {
                    DeleteItems();
                },
                () =>
                {
                    e.Handled = true;
                });
            }
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UiTool.HandleUiEvent(() =>
            {
                TModel model = this.ViewModel.LastItem;
                if (model != null && (model as IMasterDetailModel<TEntity>).IsChanged)
                {
                    ViewModel.DataSave(model);
                }

                this.currentIndex = DataGridMain.SelectedIndex;
            },
            () =>
            {
                SetSelectedIndex(currentIndex);
                return;
            });
        }

        private void SetSelectedIndex(int selectedIndex)
        {
            // From https://social.msdn.microsoft.com/Forums/vstudio/en-US/a1bf98df-4bb6-4bed-8c65-0d0246066c39/wpf-datagrid-selectionchanged-and-selectedcellschanged?forum=wpf
            DataGridMain.Dispatcher.BeginInvoke(
                new Action(() =>
                {
                    DataGridMain.SelectedIndex = selectedIndex;
                    //if (DataGrid.IsFocused)
                    DataGridMain.Focus();
                }),
                System.Windows.Threading.DispatcherPriority.Send);
        }

        private void DataGrid_AddingNewItem(object sender, AddingNewItemEventArgs e)
        {
            UiTool.HandleUiEvent(() =>
           {
               if (e.NewItem == null)
               {
                   e.NewItem = SetNewItem(new TModel());
               }
           });
        }

        private static TModel SetNewItem(TModel item)
        {
            (item as IEditableObject).BeginEdit();
            item.SetDefault();
            return item;
        }
    }
}