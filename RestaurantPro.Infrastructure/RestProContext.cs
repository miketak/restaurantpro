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

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new UserConfiguration());
        }
 
    }
        
}