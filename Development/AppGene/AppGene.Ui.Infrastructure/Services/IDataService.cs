using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppGene.Modules.Employee.Ui.Services
{
    public interface IDataService<T>
    {
        void Delete(IList<T> items);

        void Insert(T item);

        IList<T> Query();

        void Update(T item);
    }
}
