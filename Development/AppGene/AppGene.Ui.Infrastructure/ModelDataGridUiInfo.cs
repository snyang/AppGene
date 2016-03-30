using AppGene.Common.Entities.Infrastructure.Inferences;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace AppGene.Ui.Infrastructure
{
    public class ModelDataGridUiInfo
    {
        public DataGrid Grid { get; set; }
        public Dictionary<string, DataGridColumn> Columns
                { get; } = new Dictionary<string, DataGridColumn>();
    }
}
