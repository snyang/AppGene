using System.Collections.Generic;

namespace AppGene.Modules.Employee.Ui.Services
{
    public class EmployeeService : IDataService<Db.Model.Employee>
    {
        public EmployeeService()
            {
            }
        public void Delete(IList<Db.Model.Employee> employees)
        {
            AppGene.Db.EmployeeDbController.Delete(employees);
        }

        public void Insert(Db.Model.Employee employee)
        {
            AppGene.Db.EmployeeDbController.Insert(employee);
        }

        public IList<Db.Model.Employee> Query()
        {
            return AppGene.Db.EmployeeDbController.Query();
        }

        public void Update(Db.Model.Employee employee)
        {
            AppGene.Db.EmployeeDbController.Update(employee);
        }
    }
}
