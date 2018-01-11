namespace RestaurantPro.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WorkCycleStatusesAndFKConstraints : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.WcStatus",
                c => new
                    {
                        Status = c.String(nullable: false, maxLength: 15),
                    })
                .PrimaryKey(t => t.Status);
            
            AddColumn("dbo.WorkCycles", "StatusId", c => c.String(nullable: false, maxLength: 15));
            CreateIndex("dbo.WorkCycles", "StatusId");
            AddForeignKey("dbo.WorkCycles", "StatusId", "dbo.WcStatus", "Status", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WorkCycles", "StatusId", "dbo.WcStatus");
            DropIndex("dbo.WorkCycles", new[] { "StatusId" });
            DropColumn("dbo.WorkCycles", "StatusId");
            DropTable("dbo.WcStatus");
        }
    }
}
