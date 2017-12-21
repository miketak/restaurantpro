namespace RestaurantPro.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRawMaterialCatalogTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RawMaterialCatalog",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RawMaterialId = c.Int(nullable: false),
                        SupplierId = c.Int(nullable: false),
                        Price = c.Double(nullable: false),
                        UnitOfMeasure = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.RawMaterials", t => t.RawMaterialId, cascadeDelete: true)
                .ForeignKey("dbo.Suppliers", t => t.SupplierId, cascadeDelete: true)
                .Index(t => t.RawMaterialId)
                .Index(t => t.SupplierId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RawMaterialCatalog", "SupplierId", "dbo.Suppliers");
            DropForeignKey("dbo.RawMaterialCatalog", "RawMaterialId", "dbo.RawMaterials");
            DropIndex("dbo.RawMaterialCatalog", new[] { "SupplierId" });
            DropIndex("dbo.RawMaterialCatalog", new[] { "RawMaterialId" });
            DropTable("dbo.RawMaterialCatalog");
        }
    }
}
