using AppGene.Business.Infrastructure;
using AppGene.Model.EntityPerception;
using AppGene.Ui.Infrastructure;
using AppGene.Ui.Infrastructure.Mvvm;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Threading;

namespace AppGene.Ui.Patterns.MasterDetail
{

    public class MasterDetailViewModel<TEntity, TModel> : BaseViewModel
        where TEntity : class, new()
        where TModel : IMasterDetailModel<TEntity>, new()
    {
        private readonly AbstractCrudBusinessService<TEntity> businessService;
        private ListCollectionView collectionView;
        private TModel currentItem;
        private string filterString;

        DispatcherTimer filterTimer;

        public MasterDetailViewModel(AbstractCrudBusinessService<TEntity> service)
        {
            this.businessService = service;

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

        public object CurrentItem
        {
            get { return currentItem; }
            set
            {
                LastItem = currentItem;
                SetProperty(ref currentItem, As(value));
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
        public TModel LastItem { get; private set; }

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
                this.businessService.Insert(item.Entity);
                item.IsNew = false;
            }
            else
            {
                this.businessService.Update(item.Entity);
            }
            item.IsChanged = false;
            ((IEditableObject)item).EndEdit();
        }

        public IList<TModel> Delete(IList deleteItems)
        {
            IList<TEntity> entities = new List<TEntity>();
            IList<TModel> models = new List<TModel>();

            foreach (var deleteItem in deleteItems)
            {
                TModel item = As(deleteItem);
                if (item != null && !item.IsNew)
                {
                    entities.Add(item.Entity);
                    models.Add(item);
                }
            }
            if (entities.Count>0)
            { 
                this.businessService.Delete(entities);
            }
            return models;
        }

        public void GetData()
        {
            List<TModel> models = new List<TModel>();
            IList<TEntity> entities = this.businessService.Query();
            Sort(entities);
            foreach (var entity in entities)
            {
                TModel model = new TModel();
                model.Entity = entity;
                (model as IMasterDetailModel<TEntity>).IsNew = false;
                models.Add(model);
            }
            this.CollectionView = new ListCollectionView(models);
        }

        private void Sort(IList<TEntity> entities)
        {

            var sortProperties = new SortPropertyGetter().Get(new EntityAnalysisContext
            {
                EntityType = typeof(TEntity),
                Source = this.GetType().FullName,
            });

            if (sortProperties.Count == 0) return;

            EntitySortComparer<TEntity> comparer = new EntitySortComparer<TEntity>(sortProperties);
            (entities as List<TEntity>).Sort(comparer);
        }

        private TModel As(object obj)
        {
            if (obj is TModel) return (TModel)obj;
            return default(TModel);
        }

        private bool DoFilter(object filteredItem)
        {
            if (string.IsNullOrWhiteSpace(filterString)) return true;

            TModel item = As(filteredItem);
            if (item == null) return true;
            return item.DoFilter(filterString);
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
    }
}
