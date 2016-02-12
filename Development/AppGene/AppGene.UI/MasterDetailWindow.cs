using AppGene.Business.Infrastructure;
using AppGene.Ui.Patterns.MasterDetail;
using System.Windows;

namespace AppGene.Ui
{
    public class MasterDetailWindow<TEntity, TModel, TDataService> : Window
       where TEntity : class, new()
       where TModel : IMasterDetailModel<TEntity>, new()
       where TDataService : AbstractCrudBusinessService<TEntity>, new()
    {
        public MasterDetailWindow()
        {
            //InitializeComponent();
            InitializeAdapter();
        }

        public MasterDetailViewConstructor<TEntity, TModel, TDataService> Adapter { get; private set; }

        private void InitializeAdapter()
        {
            this.Adapter = new MasterDetailViewConstructor<TEntity, TModel, TDataService>(this);
            Adapter.Initialize();
        }
    }
}
