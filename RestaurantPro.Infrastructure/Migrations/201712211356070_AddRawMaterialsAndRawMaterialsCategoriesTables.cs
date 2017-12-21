namespace RestaurantPro.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRawMaterialsAndRawMaterialsCategoriesTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RawMaterialCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RawMaterials",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Measure = c.String(),
                        RawMaterialCategoryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.RawMaterialCategories", t => t.RawMaterialCategoryId, cascadeDelete: true)
                .Index(t => t.RawMaterialCategoryId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RawMaterials", "RawMaterialCategoryId", "dbo.RawMaterialCategories");
            DropIndex("dbo.RawMaterials", new[] { "RawMaterialCategoryId" });
            DropTable("dbo.RawMaterials");
            DropTable("dbo.RawMaterialCategories");
        }
    }
}
