using AppGene.Model.Patterns.Core.Layers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppGene.Ui.Patterns.GenericMvvmBusiness
{
    public interface IGenericViewModel<TEntity>
        where TEntity : class, new ()
    {
    }
}
