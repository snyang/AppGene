using AppGene.Common.Entities.Infrastructure.Inferences;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AppGene.Ui.Infrastructure
{
    public class ModelPropertyUiInfo
    {
        private const string ContentKey = "@Content";
        private const string LabelKey = "@Label";

        public ModelPropertyUiInfo(DisplayPropertyInfo displayProperty)
        {
            DisplayProperty = displayProperty;
        }

        public UIElement Content
        {
            get { return Elements[ContentKey]; }
            set
            {
                if (Elements.ContainsKey(ContentKey))
                {
                    Elements[ContentKey] = value;
                }
                else
                {
                    Elements.Add(ContentKey, value);
                }
            }
        }

        public IDictionary<string, UIElement> Elements { get; set; } = new Dictionary<string, UIElement>();
        public UIElement Label
        {
            get { return Elements[LabelKey]; }
            set
            {
                if (Elements.ContainsKey(LabelKey))
                {
                    Elements[LabelKey] = value;
                }
                else
                { 
                    Elements.Add(LabelKey, value);
                }
            }
        }

        public DisplayPropertyInfo DisplayProperty { get; private set; }

        public string PropertyName {
            get { return DisplayProperty.PropertyName; }
        }
    }
}
