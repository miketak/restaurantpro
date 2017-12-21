namespace RestaurantPro.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRawMaterialCatalogTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RawMaterialCatalogs",
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
            DropForeignKey("dbo.RawMaterialCatalogs", "SupplierId", "dbo.Suppliers");
            DropForeignKey("dbo.RawMaterialCatalogs", "RawMaterialId", "dbo.RawMaterials");
            DropIndex("dbo.RawMaterialCatalogs", new[] { "SupplierId" });
            DropIndex("dbo.RawMaterialCatalogs", new[] { "RawMaterialId" });
            DropTable("dbo.RawMaterialCatalogs");
        }
    }
}
