using AppGene.Business.Infrastructure;
using AppGene.Ui.Patterns.MasterDetail;
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

namespace AppGene.Ui.Main
{
    /// <summary>
    /// Interaction logic for MasterDetailWindow.xaml
    /// </summary>
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
