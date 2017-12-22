namespace RestaurantPro.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MakePONumberInPurchaseOrdersUnique : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PurchaseOrders", "PurchaseOrderNumber", c => c.String(nullable: false, maxLength: 10));
            CreateIndex("dbo.PurchaseOrders", "PurchaseOrderNumber", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.PurchaseOrders", new[] { "PurchaseOrderNumber" });
            AlterColumn("dbo.PurchaseOrders", "PurchaseOrderNumber", c => c.String());
        }
    }
}
