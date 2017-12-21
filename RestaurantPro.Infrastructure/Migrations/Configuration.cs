using System;
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


            var workCycles = new List<WorkCycle>
            {
                new WorkCycle{ Name = "Cycle 1", DateBegin = new DateTime(2017, 09, 01), DateEnd = new DateTime(2017, 09, 11), Active = true, UserId = 1},
                new WorkCycle{ Name = "Cycle 2", DateBegin = new DateTime(2017, 10, 02), DateEnd = new DateTime(2017, 10, 12), Active = true, UserId = 1},
                new WorkCycle{ Name = "Cycle 3", DateBegin = new DateTime(2017, 11, 03), DateEnd = new DateTime(2017, 11, 13), Active = true, UserId = 2},
                new WorkCycle{ Name = "Cycle 4", DateBegin = new DateTime(2017, 12, 04), DateEnd = new DateTime(2017, 12, 14), Active = true, UserId = 2},
                new WorkCycle{ Name = "Cycle 5", DateBegin = new DateTime(2018, 01, 15), DateEnd = new DateTime(2017, 01, 15), Active = true, UserId = 2}
            };
            workCycles.ForEach(w => context.WorkCycles.AddOrUpdate(t => t.Name, w));
            context.SaveChanges();


            //Adding Purchase Order Statuses
            var poStatuses = new List<PoStatus>
            {
                new PoStatus{ Status = "New"},
                new PoStatus{ Status = "In Progress"},
                new PoStatus{ Status = "Received"},
                new PoStatus{ Status = "Changed"},
                new PoStatus{ Status = "Closed"},
                new PoStatus{ Status = "Canceled"}
            };
            poStatuses.ForEach(p => context.PurchaseOrderStatuses.AddOrUpdate(t => t.Status, p));
            context.SaveChanges();
        }
    }
}
