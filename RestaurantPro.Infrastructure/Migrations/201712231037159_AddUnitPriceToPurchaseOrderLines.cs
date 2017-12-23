namespace RestaurantPro.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUnitPriceToPurchaseOrderLines : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PurchaseOrderLines", "UnitPrice", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PurchaseOrderLines", "UnitPrice");
        }
    }
}
