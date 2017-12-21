namespace RestaurantPro.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddWorkCycleLinesTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.WorkCycleLines",
                c => new
                    {
                        RawMaterialId = c.Int(nullable: false),
                        SupplierId = c.Int(nullable: false),
                        WorkCycleId = c.Int(nullable: false),
                        Quantity = c.Single(nullable: false),
                        UnitOfMeasure = c.String(),
                    })
                .PrimaryKey(t => new { t.RawMaterialId, t.SupplierId, t.WorkCycleId })
                .ForeignKey("dbo.Suppliers", t => t.SupplierId, cascadeDelete: true)
                .ForeignKey("dbo.RawMaterials", t => t.RawMaterialId, cascadeDelete: true)
                .ForeignKey("dbo.WorkCycles", t => t.WorkCycleId, cascadeDelete: true)
                .Index(t => t.RawMaterialId)
                .Index(t => t.SupplierId)
                .Index(t => t.WorkCycleId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WorkCycleLines", "WorkCycleId", "dbo.WorkCycles");
            DropForeignKey("dbo.WorkCycleLines", "RawMaterialId", "dbo.RawMaterials");
            DropForeignKey("dbo.WorkCycleLines", "SupplierId", "dbo.Suppliers");
            DropIndex("dbo.WorkCycleLines", new[] { "WorkCycleId" });
            DropIndex("dbo.WorkCycleLines", new[] { "SupplierId" });
            DropIndex("dbo.WorkCycleLines", new[] { "RawMaterialId" });
            DropTable("dbo.WorkCycleLines");
        }
    }
}
