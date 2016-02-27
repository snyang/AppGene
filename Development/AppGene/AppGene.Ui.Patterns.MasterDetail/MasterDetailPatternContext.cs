using AppGene.Business.Infrastructure;
using AppGene.Ui.Infrastructure.Mvvm;
using System;
using System.Windows.Controls;

namespace AppGene.Ui.Patterns.MasterDetail
{
    public class MasterDetailPatternContext<TModel, TEntity>
        where TModel : class, new()
        where TEntity : class, new()
    {
        private MasterDetailController<TModel, TEntity, DefaultEditableModel<TModel, TEntity>> viewController;

        private MasterDetailViewModel<TModel, TEntity, DefaultEditableModel<TModel, TEntity>> viewModel;

        private Type TModelType;

        public MasterDetailPatternContext(AbstractCrudBusinessService<TEntity> businessService)
        {
            if (businessService == null)
            {
                throw new ArgumentNullException("businessService");
            }
            BusinessService = businessService;
        }

        public AbstractCrudBusinessService<TEntity> BusinessService { get; set; }
        public ContentControl View { get; set; }

        public MasterDetailController<TModel, TEntity, DefaultEditableModel<TModel, TEntity>> ViewController
        {
            get
            {
                if (viewController == null)
                {
                    viewController = new MasterDetailController<TModel, TEntity, DefaultEditableModel<TModel, TEntity>>(this);
                    InitializeViewController();
                }
                return viewController;
            }
            set
            {
                viewController = value;
                InitializeViewController();
            }
        }

        public MasterDetailViewModel<TModel, TEntity, DefaultEditableModel<TModel, TEntity>> ViewModel
        {
            get
            {
                if (viewModel == null)
                {
                    viewModel = new MasterDetailViewModel<TModel, TEntity, DefaultEditableModel<TModel, TEntity>>(this);
                }
                return viewModel;
            }
            set
            {
                viewModel = value;
            }
        }

        MasterDetailUiService<TModel, TEntity> uiService;
        public MasterDetailUiService<TModel, TEntity> UiService
        {
            get
            {
                if (uiService == null)
                {
                    uiService = new MasterDetailUiService<TModel, TEntity>();
                }
                return uiService;
            }
            set
            {
                uiService = value;
            }
        }

        private void InitializeViewController()
        {
            if (View == null)
            {
                throw new InvalidOperationException("View is not set.");
            }

            if (viewController == null) return;

            viewController.Owner = View;
            viewController.ViewModel = ViewModel;
        }
    }
}