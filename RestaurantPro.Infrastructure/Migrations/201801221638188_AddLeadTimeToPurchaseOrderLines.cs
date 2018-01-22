namespace RestaurantPro.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLeadTimeToPurchaseOrderLines : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PurchaseOrderLines", "LeadTime", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PurchaseOrderLines", "LeadTime");
        }
    }
}
