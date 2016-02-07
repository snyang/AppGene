using System.Collections.Generic;

namespace AppGene.Modules.Employee.Ui.Services
{
    public class EmployeeService : IBusinessService<Db.Model.Employee>
    {
        public EmployeeService()
        {
        }
        public void Delete(IList<Db.Model.Employee> employees)
        {
            new AppGene.Db.EmployeeDbService().Delete(employees);
        }

        public void Insert(Db.Model.Employee employee)
        {
            new AppGene.Db.EmployeeDbService().Insert(employee);
        }

        public IList<Db.Model.Employee> Query()
        {
            return new AppGene.Db.EmployeeDbService().Query();
        }

        public void Update(Db.Model.Employee employee)
        {
            new AppGene.Db.EmployeeDbService().Update(employee);
        }
    }
}
