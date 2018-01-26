using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using RestaurantPro.Core.Domain;
using RestaurantPro.Infrastructure.Services;

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

            #region Add Users

            var users = new List<User>
            {
                new User {Username = "rkpadi", FirstName = "Richard", LastName = "Padi", Email = "rkpadi@yahoo.com"},
                new User {Username = "linda", FirstName = "Linda", LastName = "Ocloo", Email = "linda.ocloo@yahoo.com"},
                new User {Username = "roman", FirstName = "Roman", LastName = "Boehm", Email = "rboehm@yahoo.com"},
                new User {Username = "betty", FirstName = "Betty", LastName = "Afornu", Email = "bafornu@yahoo.com"}
            };
            users.ForEach(u => context.Users.AddOrUpdate(p => p.LastName, u));
            context.SaveChanges();

            string password = "password";

            for (int i = 1; i < 5; i++)
            {
                byte[] saltByte = Encoding.ASCII.GetBytes(RandomString());
                byte[] pasByte = Encoding.ASCII.GetBytes(password);
                byte[] passHash = Hash(pasByte, saltByte);

                var user = context.Users.SingleOrDefault(x => x.Id == i);
                user.PasswordHash = passHash;
                user.SaltHash = saltByte;
                context.SaveChanges();
            }

            #endregion

            #region Work Cycle Statuses

            var wcStatuses = new List<WcStatus>
            {
                new WcStatus {Status = WorkCycleStatus.Draft.ToString()},
                new WcStatus {Status = WorkCycleStatus.Active.ToString()},
                new WcStatus {Status = WorkCycleStatus.Closed.ToString()}
            };
            wcStatuses.ForEach(p => context.WorkCycleStatuses.AddOrUpdate(t => t.Status, p));
            context.SaveChanges();

            #endregion

            #region Add Work Cycles

            var workCycles = new List<WorkCycle>
            {
                new WorkCycle
                {
                    Name = "Cycle 1",
                    DateBegin = new DateTime(2017, 09, 01),
                    DateEnd = new DateTime(2017, 09, 11),
                    Active = true,
                    UserId = 1,
                    StatusId = WorkCycleStatus.Draft.ToString()
                },
                new WorkCycle
                {
                    Name = "Cycle 2",
                    DateBegin = new DateTime(2017, 10, 02),
                    DateEnd = new DateTime(2017, 10, 12),
                    Active = true,
                    UserId = 1,
                    StatusId = WorkCycleStatus.Draft.ToString()
                },
                new WorkCycle
                {
                    Name = "Cycle 3",
                    DateBegin = new DateTime(2017, 11, 03),
                    DateEnd = new DateTime(2017, 11, 13),
                    Active = true,
                    UserId = 2,
                    StatusId = WorkCycleStatus.Draft.ToString()
                },
                new WorkCycle
                {
                    Name = "Cycle 4",
                    DateBegin = new DateTime(2017, 12, 04),
                    DateEnd = new DateTime(2017, 12, 14),
                    Active = true,
                    UserId = 2,
                    StatusId = WorkCycleStatus.Draft.ToString()
                },
                new WorkCycle
                {
                    Name = "Cycle 5",
                    DateBegin = new DateTime(2018, 01, 15),
                    DateEnd = new DateTime(2017, 01, 15),
                    Active = true,
                    UserId = 2,
                    StatusId = WorkCycleStatus.Draft.ToString()
                }
            };
            workCycles.ForEach(w => context.WorkCycles.AddOrUpdate(t => t.Name, w));
            context.SaveChanges();

            #endregion

            #region Add Purchase Order Statuses

            //Adding Purchase Order Statuses
            var poStatuses = new List<PoStatus>
            {
                new PoStatus {Status = "New"},
                new PoStatus {Status = "In Progress"},
                new PoStatus {Status = "Received"},
                new PoStatus {Status = "Changed"},
                new PoStatus {Status = "Closed"},
                new PoStatus {Status = "Canceled"}
            };
            poStatuses.ForEach(p => context.PurchaseOrderStatuses.AddOrUpdate(t => t.Status, p));
            context.SaveChanges();

            #endregion

            #region Add Locations

            var locations = new List<Location>
            {
                new Location {LocationId = "Room A", Active = true},
                new Location {LocationId = "Room B", Active = true},
                new Location {LocationId = "Home Warehouse", Active = true}
            };
            locations.ForEach(p => context.Locations.AddOrUpdate(t => t.LocationId, p));
            context.SaveChanges();

            #endregion

            #region Add Suppliers

            var suppliers = new List<Supplier>
            {
                new Supplier
                {
                    Name = "Unassigned",
                    Address = "Not Set",
                    Telephone = "0000000000",
                    Email = "",
                    Active = true
                },
                new Supplier
                {
                    Name = "Kofi and Co Enterprise",
                    Address = "81 Miller AV",
                    Telephone = "2332345042434",
                    Email = "ma@yahoo.com",
                    Active = true
                },
                new Supplier
                {
                    Name = "Ama African Market",
                    Address = "Bung 9, 1934 Road",
                    Telephone = "3197438828",
                    Email = "ama@yahoo.com",
                    Active = true
                },
                new Supplier
                {
                    Name = "Richard Padi Ventures",
                    Address = "6301 Kirkwoord Bolevard",
                    Telephone = "123456789",
                    Email = "rkpadi@yahoo.com",
                    Active = true
                },
                new Supplier
                {
                    Name = "Zigi Industries",
                    Address = "21 Century Road",
                    Telephone = "0012354345560",
                    Email = "zigi@gmail.com",
                    Active = true
                },
                new Supplier
                {
                    Name = "Spanky Market",
                    Address = "Farmers Market Rd.",
                    Telephone = "9171243312276",
                    Email = "ddade@yahoo.com",
                    Active = true
                }
            };
            suppliers.ForEach(p => context.Suppliers.AddOrUpdate(t => t.Name, p));
            context.SaveChanges();

            #endregion

            #region Add Raw Material Categories

            var categories = new List<RawMaterialCategory>
            {
                new RawMaterialCategory {Name = "Uncategorized", Description = "Category Not Assigned Yet."},
                new RawMaterialCategory {Name = "Vegetables", Description = "Vegetable Desc"},
                new RawMaterialCategory {Name = "Fruits", Description = "Fruits Desc"},
                new RawMaterialCategory {Name = "Condiments", Description = "Condiments Desc"},
                new RawMaterialCategory {Name = "Drinks", Description = "Drinks Desc"}
            };
            categories.ForEach(p => context.RawMaterialCategories.AddOrUpdate(t => t.Name, p));
            context.SaveChanges();

            #endregion

            #region Add Raw Materials

            var rawMaterials = new List<RawMaterial>
            {
                new RawMaterial {Name = "Tomatoes", RawMaterialCategoryId = 1, Active = true},
                new RawMaterial {Name = "Cabbage", RawMaterialCategoryId = 1, Active = true},
                new RawMaterial {Name = "Sprite", RawMaterialCategoryId = 4, Active = true},
                new RawMaterial {Name = "Orange", RawMaterialCategoryId = 2, Active = true},
                new RawMaterial {Name = "Garlic", RawMaterialCategoryId = 3, Active = true}
            };
            rawMaterials.ForEach(p => context.RawMaterials.AddOrUpdate(t => t.Name, p));
            context.SaveChanges();

            #endregion

            #region Add Inventory Settings

            var inventorySettings = new List<InventorySettings>
            {
                new InventorySettings
                {
                    Parameter = "Tax",
                    Value = (decimal) 0.17500
                },
                new InventorySettings
                {
                    Parameter = "Default Lead Time",
                    Value = 5
                }

            };
            inventorySettings.ForEach( p => context.InventorySettings.AddOrUpdate(t => t.Parameter, p));
            context.SaveChanges();
            #endregion

            #region Add WorkCycle Lines

            var workCycleIds = context.WorkCycles.Select(p => p.Id).ToList();

            foreach (var workCycleId in workCycleIds)
            {
                var workCycleLinesToDb = GetWorkCycleLineWithWorkCycleIdSet(workCycleId);
                context.WorkCycleLines.AddRange(workCycleLinesToDb);
                context.SaveChanges();
            }

            #endregion

        }


        private List<WorkCycleLines> GetWorkCycleLineWithWorkCycleIdSet(int workCycleId)
        {
            var wcLines = new List<WorkCycleLines>
            {
                new WorkCycleLines
                {
                    WorkCycleId = workCycleId,
                    RawMaterialId = 1,
                    SupplierId = 1,
                    UnitPrice = 50,
                    PlannedQuantity = 45,
                    UnitOfMeasure = "crates"
                },
                new WorkCycleLines
                {
                    WorkCycleId = workCycleId,
                    RawMaterialId = 2,
                    SupplierId = 2,
                    UnitPrice = 50,
                    PlannedQuantity = 45,
                    UnitOfMeasure = "crates"
                },
                new WorkCycleLines
                {
                    WorkCycleId = workCycleId,
                    RawMaterialId = 3,
                    SupplierId = 3,
                    UnitPrice = 50,
                    PlannedQuantity = 45,
                    UnitOfMeasure = "crates"
                },
                new WorkCycleLines
                {
                    WorkCycleId = workCycleId,
                    RawMaterialId = 4,
                    SupplierId = 4,
                    UnitPrice = 50,
                    PlannedQuantity = 45,
                    UnitOfMeasure = "crates"
                },
                new WorkCycleLines
                {
                    WorkCycleId = workCycleId,
                    RawMaterialId = 5,
                    SupplierId = 5,
                    UnitPrice = 50,
                    PlannedQuantity = 45,
                    UnitOfMeasure = "crates"
                },
            };
            return wcLines;
        }

        private static Random random = new Random();
        public static string RandomString()
        {
            var length = 25;
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        //will leave soon
        public byte[] Hash(byte[] value, byte[] salt)
        {
            byte[] saltedValue = value.Concat(salt).ToArray();

            return new SHA256Managed().ComputeHash(saltedValue);
        }
    }
}
