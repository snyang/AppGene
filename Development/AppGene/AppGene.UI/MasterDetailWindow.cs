using AppGene.Db.Core;
using AppGene.Modules.Employee.Ui.Services;
using AppGene.Ui.Infrastructure.Patterns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AppGene.Ui
{
    public class MasterDetailWindow<TModel, TDataService> : Window
       where TModel : IUiModel, new()
       where TDataService : IDataService<TModel>, new()
    {
        public MasterDetailWindow()
        {
            //InitializeComponent();
            InitializeAdapter();
        }

        public MasterDetailViewAdapter<TModel, TDataService> Adapter { get; private set; }

        private void InitializeAdapter()
        {
            this.Adapter = new MasterDetailViewAdapter<TModel, TDataService>(this);
            Adapter.Initialize();
        }
    }
}
