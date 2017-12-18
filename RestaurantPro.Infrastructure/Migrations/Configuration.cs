using System.Collections.Generic;
using RestaurantPro.Core.Domain;

namespace RestaurantPro.Infrastructure.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<RestProContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(RestProContext context)
        {
            var users = new List<User>
            {
                new User{ Username = "rkpadi", FirstName = "Richard", LastName = "Padi", Email = "rkpadi@yahoo.com", Password = "password"},
                new User{ Username = "linda", FirstName = "Linda", LastName = "Ocloo", Email = "linda.ocloo@yahoo.com", Password = "password"}
            };
            users.ForEach(u => context.Users.AddOrUpdate(p => p.LastName, u));
            context.SaveChanges();
        }
    }
}
