namespace RestaurantPro.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddWorkCycleAdjustmentsTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.WorkCycleAdjustments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        WorkCycleId = c.Int(nullable: false),
                        AdjustedPlanningQty = c.Double(nullable: false),
                        TransactionDate = c.DateTime(nullable: false),
                        CreatedBy = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.WorkCycles", t => t.WorkCycleId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.CreatedBy)
                .Index(t => t.WorkCycleId)
                .Index(t => t.CreatedBy);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WorkCycleAdjustments", "CreatedBy", "dbo.Users");
            DropForeignKey("dbo.WorkCycleAdjustments", "WorkCycleId", "dbo.WorkCycles");
            DropIndex("dbo.WorkCycleAdjustments", new[] { "CreatedBy" });
            DropIndex("dbo.WorkCycleAdjustments", new[] { "WorkCycleId" });
            DropTable("dbo.WorkCycleAdjustments");
        }
    }
}
