using System.Data.Entity;
using RestaurantPro.Core.Domain;
using RestaurantPro.Infrastructure.EntityConfigurations;

namespace RestaurantPro.Infrastructure
{

    public class RestProContext : DbContext
    {
        public RestProContext()
            : base("RestProContext")
        {
            Configuration.LazyLoadingEnabled = false;
        }

        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<WorkCycle> WorkCycles { get; set; }

        public virtual DbSet<Supplier> Suppliers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new UserConfiguration());
            modelBuilder.Configurations.Add(new WorkCycleConfigurations());
        }
 
    }
        
}