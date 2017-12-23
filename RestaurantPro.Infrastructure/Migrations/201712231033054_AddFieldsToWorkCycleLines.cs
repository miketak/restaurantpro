namespace RestaurantPro.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFieldsToWorkCycleLines : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WorkCycleLines", "PlannedQuantity", c => c.Single(nullable: false));
            AddColumn("dbo.WorkCycleLines", "ActualQuantity", c => c.Single(nullable: false));
            AddColumn("dbo.WorkCycleLines", "UnitPrice", c => c.Double(nullable: false));
            DropColumn("dbo.WorkCycleLines", "Quantity");
        }
        
        public override void Down()
        {
            AddColumn("dbo.WorkCycleLines", "Quantity", c => c.Single(nullable: false));
            DropColumn("dbo.WorkCycleLines", "UnitPrice");
            DropColumn("dbo.WorkCycleLines", "ActualQuantity");
            DropColumn("dbo.WorkCycleLines", "PlannedQuantity");
        }
    }
}
