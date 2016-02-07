using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppGene.Db.Core
{
    public class UiModelView
    {
        public IList<UiModelViewColumn> Columns { get; set; }

        public String Text { get; set; }

    }
}
