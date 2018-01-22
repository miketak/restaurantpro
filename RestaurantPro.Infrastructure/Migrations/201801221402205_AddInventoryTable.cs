namespace RestaurantPro.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddInventoryTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Inventory",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RawMaterialId = c.Int(nullable: false),
                        InitialQuantity = c.Double(nullable: false),
                        CurrentQuantity = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.RawMaterials", t => t.RawMaterialId, cascadeDelete: true)
                .Index(t => t.RawMaterialId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Inventory", "RawMaterialId", "dbo.RawMaterials");
            DropIndex("dbo.Inventory", new[] { "RawMaterialId" });
            DropTable("dbo.Inventory");
        }
    }
}
