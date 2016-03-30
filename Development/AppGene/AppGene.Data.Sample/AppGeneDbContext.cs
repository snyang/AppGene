using AppGene.Common.Entities;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace AppGene.Data.Sample
{
    public class AppGeneDbContext : DbContext
    {
        public AppGeneDbContext()
            : base("name=AppGeneDbContext")
        {
            Database.SetInitializer<AppGeneDbContext>(new AppGeneDbContextInitializer());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingEntitySetNameConvention>();
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        public virtual DbSet<Employee> Employees { get; set; }

        public virtual DbSet<DataTypeGroupA> DataTypeGroupA { get; set; }
    }
}