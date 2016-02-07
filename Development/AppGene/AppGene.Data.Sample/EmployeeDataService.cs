using AppGene.Data.Infrastructure;
using AppGene.Model.Entities;
using System.Data.Entity;
using System.Linq;

namespace AppGene.Data.Sample
{
    public class EmployeeDataService
        : AbstractDataService<AppGeneDbContext, Employee>
    {
        public override DbSet<Employee> GetDbSet(AppGeneDbContext context)
        {
            return context.Employees;
        }

        public override IQueryable<Employee> Sort(DbSet<Employee> entities)
        {
            return entities.OrderBy(e => e.EmployeeCode);
        }        
    }
}
