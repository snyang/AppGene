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
    public class ModelUiInfo
    {
        public Grid Grid { get; set; }
        public Dictionary<string, ModelPropertyUiInfo> PropertyUiInfos
                { get; } = new Dictionary<string, ModelPropertyUiInfo>();
    }
}
