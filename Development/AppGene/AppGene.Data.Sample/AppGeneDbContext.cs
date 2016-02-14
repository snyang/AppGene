using AppGene.Model.Entities;
using System.Data.Entity;

namespace AppGene.Data.Sample
{
    public partial class AppGeneDbContext : DbContext
    {
        public AppGeneDbContext()
            : base("name=AppGeneDbContext")
        {
            Database.SetInitializer<AppGeneDbContext>(new AppGeneDbContextInitializer());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }

        public virtual DbSet<Employee> Employees { get; set; }
    }
}