using System.Windows;

namespace AppGene.Ui.Patterns.MasterDetail
{
    /// <summary>
    /// Interaction logic for MasterDetailView.xaml
    /// </summary>
    public partial class MasterDetailView<TEntity, TModel> : Window
          where TEntity : class, new()
          where TModel : IMasterDetailModel<TEntity>, new()
    {
        public MasterDetailView(MasterDetailPatternContext<TEntity, TModel> patternContext)
        {
            //InitializeComponent();
            Initialize(patternContext);
        }

        public MasterDetailViewConstructor<TEntity, TModel> Constructor { get; private set; }

        private void Initialize(MasterDetailPatternContext<TEntity, TModel> patternContext)
        {
            patternContext.View = this;
            this.Constructor = new MasterDetailViewConstructor<TEntity, TModel>(patternContext, this);
            Constructor.Initialize();
        }
    }
}