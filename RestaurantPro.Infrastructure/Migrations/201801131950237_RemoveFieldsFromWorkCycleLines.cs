namespace RestaurantPro.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveFieldsFromWorkCycleLines : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.WorkCycleLines", "ActualQuantity");
            DropColumn("dbo.WorkCycleLines", "CurrentQuantity");
        }
        
        public override void Down()
        {
            AddColumn("dbo.WorkCycleLines", "CurrentQuantity", c => c.Single());
            AddColumn("dbo.WorkCycleLines", "ActualQuantity", c => c.Single());
        }
    }
}
