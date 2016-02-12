using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppGene.Ui.Patterns.GenericMvvmBusiness
{
    public interface IGenericViewConstructor<TEntity>
        where TEntity : class, new()
    {
        IGenericLayoutDescriptor<TEntity> LayoutDescriptor { get; set;}
    }
}
