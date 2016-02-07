using AppGene.Business.Infrastructure;
using AppGene.Data.Sample;
using AppGene.Model.Entities;
using System.Collections.Generic;

namespace AppGene.Business.Sample
{
    public class EmployeeService : ICommonBusinessService<Employee>
    {
        public EmployeeService()
        {
        }
        public void Delete(IList<Employee> employees)
        {
            new EmployeeDataService().Delete(employees);
        }

        public void Insert(Employee employee)
        {
            new EmployeeDataService().Insert(employee);
        }

        public IList<Employee> Query()
        {
            return new EmployeeDataService().Query();
        }

        public void Update(Employee employee)
        {
            new EmployeeDataService().Update(employee);
        }
    }
}
