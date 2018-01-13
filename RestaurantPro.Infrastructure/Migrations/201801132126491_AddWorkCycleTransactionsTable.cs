namespace RestaurantPro.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddWorkCycleTransactionsTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.WorkCycleTransactions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        WorkCycleId = c.Int(nullable: false),
                        RawMaterialId = c.Int(nullable: false),
                        TrackingNumber = c.Int(nullable: false),
                        UsedQuantity = c.Double(nullable: false),
                        DateUsed = c.DateTime(nullable: false),
                        CreatedBy = c.Int(nullable: false),
                        TransactionDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.RawMaterials", t => t.RawMaterialId, cascadeDelete: true)
                .ForeignKey("dbo.WorkCycles", t => t.WorkCycleId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.CreatedBy)
                .Index(t => t.WorkCycleId)
                .Index(t => t.RawMaterialId)
                .Index(t => t.CreatedBy);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WorkCycleTransactions", "CreatedBy", "dbo.Users");
            DropForeignKey("dbo.WorkCycleTransactions", "WorkCycleId", "dbo.WorkCycles");
            DropForeignKey("dbo.WorkCycleTransactions", "RawMaterialId", "dbo.RawMaterials");
            DropIndex("dbo.WorkCycleTransactions", new[] { "CreatedBy" });
            DropIndex("dbo.WorkCycleTransactions", new[] { "RawMaterialId" });
            DropIndex("dbo.WorkCycleTransactions", new[] { "WorkCycleId" });
            DropTable("dbo.WorkCycleTransactions");
        }
    }
}
