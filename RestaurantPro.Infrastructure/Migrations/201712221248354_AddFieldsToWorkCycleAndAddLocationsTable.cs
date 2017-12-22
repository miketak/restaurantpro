namespace RestaurantPro.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFieldsToWorkCycleAndAddLocationsTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Locations",
                c => new
                    {
                        LocationId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.LocationId);
            
            AddColumn("dbo.WorkCycleLines", "CurrentQuanity", c => c.Single(nullable: false));
            AddColumn("dbo.WorkCycleLines", "WasMoved", c => c.Boolean(nullable: false));
            AddColumn("dbo.WorkCycleLines", "MoveDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.WorkCycleLines", "LocationId", c => c.String(maxLength: 128));
            CreateIndex("dbo.WorkCycleLines", "LocationId");
            AddForeignKey("dbo.WorkCycleLines", "LocationId", "dbo.Locations", "LocationId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WorkCycleLines", "LocationId", "dbo.Locations");
            DropIndex("dbo.WorkCycleLines", new[] { "LocationId" });
            DropColumn("dbo.WorkCycleLines", "LocationId");
            DropColumn("dbo.WorkCycleLines", "MoveDate");
            DropColumn("dbo.WorkCycleLines", "WasMoved");
            DropColumn("dbo.WorkCycleLines", "CurrentQuanity");
            DropTable("dbo.Locations");
        }
    }
}
