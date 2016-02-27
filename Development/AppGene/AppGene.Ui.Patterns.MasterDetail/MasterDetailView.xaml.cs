using System.Windows;

namespace AppGene.Ui.Patterns.MasterDetail
{
    /// <summary>
    /// Interaction logic for MasterDetailView.xaml
    /// </summary>
    public partial class MasterDetailView<TModel, TEntity> : Window
          where TModel : class, new()
          where TEntity :class, new()
    {
        public MasterDetailView(MasterDetailPatternContext<TModel, TEntity> patternContext)
        {
            //InitializeComponent();
            Initialize(patternContext);
        }

        public MasterDetailViewConstructor<TModel, TEntity> Constructor { get; private set; }

        private void Initialize(MasterDetailPatternContext<TModel, TEntity> patternContext)
        {
            patternContext.View = this;
            this.Constructor = new MasterDetailViewConstructor<TModel, TEntity>(patternContext, this);
            Constructor.Initialize();
        }
    }
}