using AppGene.Business.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
