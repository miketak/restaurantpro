namespace RestaurantPro.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreateTwo : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Locations",
                c => new
                    {
                        LocationId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.LocationId);
            
            CreateTable(
                "dbo.PurchaseOrderLines",
                c => new
                    {
                        PurchaseOrderId = c.Int(nullable: false),
                        SupplierId = c.Int(nullable: false),
                        RawMaterialId = c.Int(nullable: false),
                        UnitPrice = c.Double(nullable: false),
                        Quantity = c.Single(nullable: false),
                        UnitOfMeasure = c.String(),
                    })
                .PrimaryKey(t => new { t.PurchaseOrderId, t.SupplierId, t.RawMaterialId })
                .ForeignKey("dbo.PurchaseOrders", t => t.PurchaseOrderId, cascadeDelete: true)
                .ForeignKey("dbo.RawMaterials", t => t.RawMaterialId, cascadeDelete: true)
                .ForeignKey("dbo.Suppliers", t => t.SupplierId, cascadeDelete: true)
                .Index(t => t.PurchaseOrderId)
                .Index(t => t.SupplierId)
                .Index(t => t.RawMaterialId);
            
            CreateTable(
                "dbo.PurchaseOrders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PurchaseOrderNumber = c.String(nullable: false, maxLength: 10),
                        DateCreated = c.DateTime(nullable: false),
                        CreatedBy = c.Int(nullable: false),
                        StatusId = c.String(nullable: false, maxLength: 15),
                        WorkCycleId = c.Int(nullable: false),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PoStatus", t => t.StatusId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.CreatedBy, cascadeDelete: true)
                .ForeignKey("dbo.WorkCycles", t => t.WorkCycleId)
                .Index(t => t.PurchaseOrderNumber, unique: true)
                .Index(t => t.CreatedBy)
                .Index(t => t.StatusId)
                .Index(t => t.WorkCycleId);
            
            CreateTable(
                "dbo.PoStatus",
                c => new
                    {
                        Status = c.String(nullable: false, maxLength: 15),
                    })
                .PrimaryKey(t => t.Status);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Username = c.String(nullable: false, maxLength: 10),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Email = c.String(nullable: false, maxLength: 64),
                        Password = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Username, unique: true)
                .Index(t => t.Email, unique: true);
            
            CreateTable(
                "dbo.WorkCycles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 24),
                        DateBegin = c.DateTime(nullable: false),
                        DateEnd = c.DateTime(nullable: false),
                        Active = c.Boolean(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.Name, unique: true, name: "IX_WorkCycleName")
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.WorkCycleLines",
                c => new
                    {
                        RawMaterialId = c.Int(nullable: false),
                        SupplierId = c.Int(nullable: false),
                        WorkCycleId = c.Int(nullable: false),
                        PlannedQuantity = c.Single(nullable: false),
                        ActualQuantity = c.Single(),
                        CurrentQuantity = c.Single(),
                        UnitPrice = c.Double(nullable: false),
                        UnitOfMeasure = c.String(),
                        WasMoved = c.Boolean(nullable: false),
                        MoveDate = c.DateTime(),
                        LocationId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => new { t.RawMaterialId, t.SupplierId, t.WorkCycleId })
                .ForeignKey("dbo.Locations", t => t.LocationId)
                .ForeignKey("dbo.Suppliers", t => t.SupplierId, cascadeDelete: true)
                .ForeignKey("dbo.RawMaterials", t => t.RawMaterialId, cascadeDelete: true)
                .ForeignKey("dbo.WorkCycles", t => t.WorkCycleId, cascadeDelete: true)
                .Index(t => t.RawMaterialId)
                .Index(t => t.SupplierId)
                .Index(t => t.WorkCycleId)
                .Index(t => t.LocationId);
            
            CreateTable(
                "dbo.RawMaterials",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        RawMaterialCategoryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.RawMaterialCategories", t => t.RawMaterialCategoryId, cascadeDelete: true)
                .Index(t => t.RawMaterialCategoryId);
            
            CreateTable(
                "dbo.RawMaterialCatalog",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RawMaterialId = c.Int(nullable: false),
                        SupplierId = c.Int(nullable: false),
                        Price = c.Double(nullable: false),
                        UnitOfMeasure = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.RawMaterials", t => t.RawMaterialId, cascadeDelete: true)
                .ForeignKey("dbo.Suppliers", t => t.SupplierId, cascadeDelete: true)
                .Index(t => t.RawMaterialId)
                .Index(t => t.SupplierId);
            
            CreateTable(
                "dbo.Suppliers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Address = c.String(),
                        Telephone = c.String(),
                        Email = c.String(),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RawMaterialCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WorkCycleLines", "WorkCycleId", "dbo.WorkCycles");
            DropForeignKey("dbo.WorkCycleLines", "RawMaterialId", "dbo.RawMaterials");
            DropForeignKey("dbo.RawMaterials", "RawMaterialCategoryId", "dbo.RawMaterialCategories");
            DropForeignKey("dbo.WorkCycleLines", "SupplierId", "dbo.Suppliers");
            DropForeignKey("dbo.RawMaterialCatalog", "SupplierId", "dbo.Suppliers");
            DropForeignKey("dbo.PurchaseOrderLines", "SupplierId", "dbo.Suppliers");
            DropForeignKey("dbo.RawMaterialCatalog", "RawMaterialId", "dbo.RawMaterials");
            DropForeignKey("dbo.PurchaseOrderLines", "RawMaterialId", "dbo.RawMaterials");
            DropForeignKey("dbo.WorkCycleLines", "LocationId", "dbo.Locations");
            DropForeignKey("dbo.WorkCycles", "UserId", "dbo.Users");
            DropForeignKey("dbo.PurchaseOrders", "WorkCycleId", "dbo.WorkCycles");
            DropForeignKey("dbo.PurchaseOrders", "CreatedBy", "dbo.Users");
            DropForeignKey("dbo.PurchaseOrders", "StatusId", "dbo.PoStatus");
            DropForeignKey("dbo.PurchaseOrderLines", "PurchaseOrderId", "dbo.PurchaseOrders");
            DropIndex("dbo.RawMaterialCatalog", new[] { "SupplierId" });
            DropIndex("dbo.RawMaterialCatalog", new[] { "RawMaterialId" });
            DropIndex("dbo.RawMaterials", new[] { "RawMaterialCategoryId" });
            DropIndex("dbo.WorkCycleLines", new[] { "LocationId" });
            DropIndex("dbo.WorkCycleLines", new[] { "WorkCycleId" });
            DropIndex("dbo.WorkCycleLines", new[] { "SupplierId" });
            DropIndex("dbo.WorkCycleLines", new[] { "RawMaterialId" });
            DropIndex("dbo.WorkCycles", new[] { "UserId" });
            DropIndex("dbo.WorkCycles", "IX_WorkCycleName");
            DropIndex("dbo.Users", new[] { "Email" });
            DropIndex("dbo.Users", new[] { "Username" });
            DropIndex("dbo.PurchaseOrders", new[] { "WorkCycleId" });
            DropIndex("dbo.PurchaseOrders", new[] { "StatusId" });
            DropIndex("dbo.PurchaseOrders", new[] { "CreatedBy" });
            DropIndex("dbo.PurchaseOrders", new[] { "PurchaseOrderNumber" });
            DropIndex("dbo.PurchaseOrderLines", new[] { "RawMaterialId" });
            DropIndex("dbo.PurchaseOrderLines", new[] { "SupplierId" });
            DropIndex("dbo.PurchaseOrderLines", new[] { "PurchaseOrderId" });
            DropTable("dbo.RawMaterialCategories");
            DropTable("dbo.Suppliers");
            DropTable("dbo.RawMaterialCatalog");
            DropTable("dbo.RawMaterials");
            DropTable("dbo.WorkCycleLines");
            DropTable("dbo.WorkCycles");
            DropTable("dbo.Users");
            DropTable("dbo.PoStatus");
            DropTable("dbo.PurchaseOrders");
            DropTable("dbo.PurchaseOrderLines");
            DropTable("dbo.Locations");
        }
    }
}
