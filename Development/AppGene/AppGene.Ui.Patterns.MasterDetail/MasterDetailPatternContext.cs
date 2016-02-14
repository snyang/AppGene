using AppGene.Business.Infrastructure;
using System;
using System.Windows.Controls;

namespace AppGene.Ui.Patterns.MasterDetail
{
    public class MasterDetailPatternContext<TEntity, TModel>
        where TEntity : class, new()
        where TModel : IMasterDetailModel<TEntity>, new()
    {
        private MasterDetailController<TEntity, TModel> viewController;

        private MasterDetailViewModel<TEntity, TModel> viewModel;

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

        public MasterDetailController<TEntity, TModel> ViewController
        {
            get
            {
                if (viewController == null)
                {
                    viewController = new MasterDetailController<TEntity, TModel>();
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

        public MasterDetailViewModel<TEntity, TModel> ViewModel
        {
            get
            {
                if (viewModel == null)
                {
                    viewModel = new MasterDetailViewModel<TEntity, TModel>(BusinessService);
                }
                return viewModel;
            }
            set
            {
                viewModel = value;
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