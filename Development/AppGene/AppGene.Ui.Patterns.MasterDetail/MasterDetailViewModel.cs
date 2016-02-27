using AppGene.Business.Infrastructure;
using AppGene.Common.Entities.Infrastructure.EntityModels;
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
    public class MasterDetailViewModel<TModel, TEntity, TModelAdapter> : BaseViewModel
        where TModel : class, new()
        where TEntity : class, new()
        where TModelAdapter: DefaultEditableModel<TModel, TEntity>, new()
    {
        private readonly AbstractCrudBusinessService<TEntity> businessService;
        private ListCollectionView collectionView;
        private MasterDetailPatternContext<TModel, TEntity> context;
        private TModelAdapter currentItem;
        private string filterString;

        private DispatcherTimer filterTimer;

        public MasterDetailViewModel(MasterDetailPatternContext<TModel, TEntity> context)
        {
            this.context = context;
            this.businessService = context.BusinessService;

            this.CollectionView = new ListCollectionView(new List<TModelAdapter>());
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
                SetProperty(ref currentItem, AsModelAdapter(value));
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
                    || this.currentItem.ToIEditableModel().IsNew)
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
                    var firstChangedItem = this.CollectionView.OfType<TModelAdapter>().First(item => item.ToIEditableModel().IsChanged == true);
                    return !(firstChangedItem == null);
                }
                catch
                {
                    return false;
                }
            }
        }

        public TModelAdapter LastItem { get; private set; }

        public void DataSave(TModelAdapter item)
        {
            // validation
            String validateErrors = ((IDataErrorInfo)item).Error;
            if (!string.IsNullOrEmpty(validateErrors))
            {
                MessageBox.Show(validateErrors);
                return;
            }
            // save data
            if (item.ToIEditableModel().IsNew)
            {
                this.businessService.Insert(item.Entity);
                item.ToIEditableModel().IsNew = false;
            }
            else
            {
                this.businessService.Update(item.Entity);
            }
            item.ToIEditableModel().IsChanged = false;
            ((IEditableObject)item).EndEdit();
        }

        public IList<TModelAdapter> Delete(IList deleteItems)
        {
            IList<TEntity> entities = new List<TEntity>();
            IList<TModelAdapter> models = new List<TModelAdapter>();

            foreach (var deleteItem in deleteItems)
            {
                TModelAdapter item = AsModelAdapter(deleteItem);
                if (item != null && !item.ToIEditableModel().IsNew)
                {
                    entities.Add(item.Entity);
                    models.Add(item);
                }
            }
            if (entities.Count > 0)
            {
                this.businessService.Delete(entities);
            }
            return models;
        }

        public void GetData()
        {
            IList<TEntity> entities = this.businessService.Query();
            context.UiService.Sort(entities);

            List<TModelAdapter> models = new List<TModelAdapter>();
            foreach (var entity in entities)
            {
                TModelAdapter model = EntityModelHelper.TryDynamicCreateModel<TModelAdapter, TEntity>(entity);
                models.Add(model);
            }
            this.CollectionView = new ListCollectionView(models);
        }

        private static TModelAdapter AsModelAdapter(object obj)
        {
            if (obj is TModelAdapter) return (TModelAdapter)obj;
            return default(TModelAdapter);
        }

        private bool DoFilter(object filteredItem)
        {
            if (string.IsNullOrWhiteSpace(filterString)) return true;

            TModelAdapter item = AsModelAdapter(filteredItem);
            if (item == null) return true;
            return context.UiService.DoFilter(item.ToIEditableModel().Model, filterString);
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