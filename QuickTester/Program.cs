using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestaurantPro.Core.Domain;
using RestaurantPro.Infrastructure;

namespace QuickTester
{
    class Program
    {
        static void Main(string[] args)
        {
            RestProContext context = new RestProContext();

            var po = new PurchaseOrder
            {
                PurchaseOrderNumber = "100-LA",
                DateCreated = DateTime.Now,
                CreatedBy = 1,
                StatusId = "New",
                Active = true,
                WorkCycleId = 1
            };

            context.PurchaseOrders.Add(po);
            context.SaveChanges();

            Console.WriteLine("Successful");
            Console.ReadKey();
        }
    }
}
