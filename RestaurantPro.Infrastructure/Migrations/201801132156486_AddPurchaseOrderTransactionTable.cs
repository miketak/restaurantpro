namespace RestaurantPro.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPurchaseOrderTransactionTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PurchaseOrderTransactions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PurchaseOrderId = c.Int(nullable: false),
                        RawMaterialId = c.Int(nullable: false),
                        SupplierId = c.Int(nullable: false),
                        TrackingNumber = c.Int(nullable: false),
                        QuantityReceived = c.Double(nullable: false),
                        DateReceived = c.DateTime(nullable: false),
                        DeliveredBy = c.String(),
                        ReceivedBy = c.Int(nullable: false),
                        TransactionDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PurchaseOrders", t => t.PurchaseOrderId, cascadeDelete: true)
                .ForeignKey("dbo.RawMaterials", t => t.RawMaterialId, cascadeDelete: true)
                .ForeignKey("dbo.Suppliers", t => t.SupplierId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.ReceivedBy)
                .Index(t => t.PurchaseOrderId)
                .Index(t => t.RawMaterialId)
                .Index(t => t.SupplierId)
                .Index(t => t.ReceivedBy);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PurchaseOrderTransactions", "ReceivedBy", "dbo.Users");
            DropForeignKey("dbo.PurchaseOrderTransactions", "SupplierId", "dbo.Suppliers");
            DropForeignKey("dbo.PurchaseOrderTransactions", "RawMaterialId", "dbo.RawMaterials");
            DropForeignKey("dbo.PurchaseOrderTransactions", "PurchaseOrderId", "dbo.PurchaseOrders");
            DropIndex("dbo.PurchaseOrderTransactions", new[] { "ReceivedBy" });
            DropIndex("dbo.PurchaseOrderTransactions", new[] { "SupplierId" });
            DropIndex("dbo.PurchaseOrderTransactions", new[] { "RawMaterialId" });
            DropIndex("dbo.PurchaseOrderTransactions", new[] { "PurchaseOrderId" });
            DropTable("dbo.PurchaseOrderTransactions");
        }
    }
}
