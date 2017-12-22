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
            //RestProContext context = new RestProContext();

            //Adding one purchase order with lines
            //var po = new PurchaseOrder
            //{
            //    PurchaseOrderNumber = "104-LA",
            //    DateCreated = DateTime.Now,
            //    CreatedBy = 1,
            //    StatusId = "New",
            //    Active = true,
            //    WorkCycleId = 23
            //};

            //var poLines = new List<PurchaseOrderLine>
            //{
            //    new PurchaseOrderLine { PurchaseOrderId = 7, RawMaterialId = 1, SupplierId = 3, Quantity = 60, UnitOfMeasure = "crates"},
            //    new PurchaseOrderLine { PurchaseOrderId = 7, RawMaterialId = 2, SupplierId = 4, Quantity = 60, UnitOfMeasure = "bags"},
            //    new PurchaseOrderLine { PurchaseOrderId = 7, RawMaterialId = 3, SupplierId = 5, Quantity = 60, UnitOfMeasure = "olonkas"},
            //    new PurchaseOrderLine { PurchaseOrderId = 7, RawMaterialId = 4, SupplierId = 6, Quantity = 60, UnitOfMeasure = "cups"},
            //    new PurchaseOrderLine { PurchaseOrderId = 7, RawMaterialId = 5, SupplierId = 7, Quantity = 60, UnitOfMeasure = "container"},
            //    new PurchaseOrderLine { PurchaseOrderId = 7, RawMaterialId = 2, SupplierId = 3, Quantity = 60, UnitOfMeasure = "kg"}
            //};
            //po.Lines = poLines;

            Console.WriteLine("Successful");
            Console.ReadKey();
        }
    }
}
