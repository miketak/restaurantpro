namespace RestaurantPro.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PurchaseOrderLinesTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PurchaseOrderLines",
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
            DropForeignKey("dbo.PurchaseOrderLines", "SupplierId", "dbo.Suppliers");
            DropForeignKey("dbo.PurchaseOrderLines", "RawMaterialId", "dbo.RawMaterials");
            DropForeignKey("dbo.PurchaseOrderLines", "PurchaseOrderId", "dbo.PurchaseOrders");
            DropIndex("dbo.PurchaseOrderLines", new[] { "RawMaterialId" });
            DropIndex("dbo.PurchaseOrderLines", new[] { "SupplierId" });
            DropIndex("dbo.PurchaseOrderLines", new[] { "PurchaseOrderId" });
            DropTable("dbo.PurchaseOrderLines");
        }
    }
}
