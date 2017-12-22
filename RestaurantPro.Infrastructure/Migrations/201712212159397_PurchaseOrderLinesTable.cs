namespace RestaurantPro.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PurchaseOrderLinesTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PurchaseOrderLine",
                c => new
                    {
                        PurchaseOrderId = c.Int(nullable: false),
                        SupplierId = c.Int(nullable: false),
                        RawMaterialId = c.Int(nullable: false),
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PurchaseOrderLine", "SupplierId", "dbo.Suppliers");
            DropForeignKey("dbo.PurchaseOrderLine", "RawMaterialId", "dbo.RawMaterials");
            DropForeignKey("dbo.PurchaseOrderLine", "PurchaseOrderId", "dbo.PurchaseOrders");
            DropIndex("dbo.PurchaseOrderLine", new[] { "RawMaterialId" });
            DropIndex("dbo.PurchaseOrderLine", new[] { "SupplierId" });
            DropIndex("dbo.PurchaseOrderLine", new[] { "PurchaseOrderId" });
            DropTable("dbo.PurchaseOrderLine");
        }
    }
}
