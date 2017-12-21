namespace RestaurantPro.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddWorkCycleFKToPurchaseOrdersTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PurchaseOrders", "WorkCycleId", c => c.Int(nullable: false));
            CreateIndex("dbo.PurchaseOrders", "WorkCycleId");
            AddForeignKey("dbo.PurchaseOrders", "WorkCycleId", "dbo.WorkCycles", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PurchaseOrders", "WorkCycleId", "dbo.WorkCycles");
            DropIndex("dbo.PurchaseOrders", new[] { "WorkCycleId" });
            DropColumn("dbo.PurchaseOrders", "WorkCycleId");
        }
    }
}
