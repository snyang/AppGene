using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Objects;
using System.Data.Entity;
using AppGene.Db.Model;

namespace AppGene.Db
{
    public class EmployeeDbController
    {
         public static IList<Employee> Query()
        {
            using (var context = new AppGeneDbContext())
            {

                var employees = context.Employees.OrderBy(e => e.EmployeeCode);
                return employees.ToList<Employee>();
            }
        }

        public static void Insert(Employee employee)
        {
            using (var context = new AppGeneDbContext())
            {
                context.Employees.Add(employee);
                context.SaveChanges();
            }
        }

        public static void Update(Employee employee)
        {
            using (var context = new AppGeneDbContext())
            {
                context.Employees.Attach(employee);
                context.Entry(employee).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public static void Delete(IList<Employee> employees)
        {
            using (var context = new AppGeneDbContext())
            {
                foreach (var employee in employees)
                { 
                    context.Employees.Attach(employee);
                    context.Entry(employee).State = EntityState.Deleted;
                }

                context.SaveChanges();
            }
        }
    }
}
