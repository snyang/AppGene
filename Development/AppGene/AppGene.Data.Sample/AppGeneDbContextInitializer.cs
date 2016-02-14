using AppGene.Model.Entities;
using System;
using System.Data.Entity;

namespace AppGene.Data.Sample
{
    public class AppGeneDbContextInitializer : DropCreateDatabaseIfModelChanges<AppGeneDbContext>
    //DropCreateDatabaseAlways<AppGeneDbContext>
    {
        protected override void Seed(AppGeneDbContext context)
        {
            Employee employee = new Employee();
            employee.EmployeeCode = 1;
            employee.EmployeeName = "Test User 1";
            employee.Gender = Genders.Male;
            employee.Birthday = new DateTime(1975, 5, 1);

            context.Employees.Add(employee);
            base.Seed(context);
        }
    }
}