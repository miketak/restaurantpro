namespace RestaurantPro.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MakeMoveDateNullableInWorkCycleLinesTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WorkCycleLines", "CurrentQuantity", c => c.Single(nullable: false));
            AlterColumn("dbo.WorkCycleLines", "MoveDate", c => c.DateTime());
            DropColumn("dbo.WorkCycleLines", "CurrentQuanity");
        }
        
        public override void Down()
        {
            AddColumn("dbo.WorkCycleLines", "CurrentQuanity", c => c.Single(nullable: false));
            AlterColumn("dbo.WorkCycleLines", "MoveDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.WorkCycleLines", "CurrentQuantity");
        }
    }
}
