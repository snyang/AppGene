using AppGene.Db.Core;
using AppGene.Modules.Employee.Ui.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Threading;

namespace AppGene.Ui.Infrastructure.Patterns
{

    public class MasterDetailViewModel<TModel> : ViewModelBase
        where TModel : IUiModel
    {
        private TModel currentItem;
        private readonly IDataService<TModel> dataService;
        private ListCollectionView collectionView;
        private string filterString;

        public MasterDetailViewModel(IDataService<TModel> dataService)
        {
            this.dataService = dataService;

            this.CollectionView = new ListCollectionView(new List<TModel>());
        }

        public ListCollectionView CollectionView
        {
            get
            {
                return collectionView;
            }
            set
            {
                collectionView = value;
                this.OnPropertyChanged("CollectionView");
            }
        }

        public bool IsAddingNew
        {
            get
            {
                if (this.CurrentItem == null
                    || this.currentItem.IsNew)
                {
                    return true;
                }

                return false;
            }
        }

        public bool IsChanged
        {
            get
            {
                try
                {
                    var firstChangedItem = this.CollectionView.OfType<TModel>().First(item => item.IsChanged == true);
                    return !(firstChangedItem == null);
                }
                catch
                {
                    return false;
                }
            }
        }

        public string FilterString
        {
            get { return filterString; }
            set
            {
                filterString = value;
                if (string.IsNullOrWhiteSpace(filterString))
                {
                    StopFilter();
                }
                else
                {
                    StartFilter();
                }
            }
        }

        DispatcherTimer filterTimer;
        private void StartFilter()
        {
            lock (this)
            {
                if (filterTimer != null)
                {
                    filterTimer.Stop();
                    filterTimer = null;
                }
                if (filterTimer == null)
                {
                    filterTimer = new DispatcherTimer();
                    filterTimer.Tick += new EventHandler(filterTimer_Tick);
                    filterTimer.Interval = new TimeSpan(0, 0, 0, 0, 200);
                    filterTimer.Start();
                }
            }
        }

        private void filterTimer_Tick(object sender, EventArgs e)
        {
            lock (this)
            {
                if (filterTimer != null)
                {
                    filterTimer.Stop();
                    filterTimer = null;
                }
                if (CollectionView.Filter == null)
                {
                    CollectionView.Filter = DoFilter;
                }
                CollectionView.Refresh();
            }
        }

        private void StopFilter()
        {
            lock (this)
            {
                if (filterTimer != null)
                {
                    filterTimer.Stop();
                    filterTimer = null;
                }
                if (CollectionView.Filter != null)
                {
                    CollectionView.Filter = null;
                    CollectionView.Refresh();
                }
            }
        }

        private bool DoFilter(object filteredItem)
        {
            if (string.IsNullOrWhiteSpace(filterString)) return true;

            TModel item = As(filteredItem);
            if (item == null) return true;
            return item.DoFilter(filterString);
        }

        public IList<TModel> Delete(IList deleteItems)
        {
            IList<TModel> items = new List<TModel>();

            foreach (var deleteItem in deleteItems)
            {
                TModel item = As(deleteItem);
                if (item != null && item.IsNew)
                {
                    items.Add(item);
                }
            }

            this.dataService.Delete(items);
            return items;
        }

        public void DataSave(TModel item)
        {
            // validation
            String validateErrors = ((IDataErrorInfo)item).Error;
            if (!string.IsNullOrEmpty(validateErrors))
            {
                MessageBox.Show(validateErrors);
                return;
            }
            // save data
            if (item.IsNew)
            {
                this.dataService.Insert(item);
            }
            else
            {
                this.dataService.Update(item);
            }
            item.IsChanged = false;
            ((IEditableObject)item).EndEdit();
        }

        public void GetData()
        {
            this.CollectionView = new ListCollectionView(this.dataService.Query() as IList);
        }
                
        public TModel LastItem { get; private set; }
        public object CurrentItem
        {
            get { return currentItem; }
            set
            {
                LastItem = currentItem;
                SetProperty(ref currentItem, As(value));
            }
        }

        private TModel As(object obj)
        {
            if (obj is TModel) return (TModel)obj;
            return default(TModel);
        }
    }
}
