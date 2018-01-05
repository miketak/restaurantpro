namespace RestaurantPro.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MakeWorkCycleIdInPurchaseOrdersNullable : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.PurchaseOrders", new[] { "WorkCycleId" });
            AlterColumn("dbo.PurchaseOrders", "WorkCycleId", c => c.Int());
            CreateIndex("dbo.PurchaseOrders", "WorkCycleId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.PurchaseOrders", new[] { "WorkCycleId" });
            AlterColumn("dbo.PurchaseOrders", "WorkCycleId", c => c.Int(nullable: false));
            CreateIndex("dbo.PurchaseOrders", "WorkCycleId");
        }
    }
}
