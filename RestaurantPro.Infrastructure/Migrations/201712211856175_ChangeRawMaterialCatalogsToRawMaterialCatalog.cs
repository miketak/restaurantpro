namespace RestaurantPro.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeRawMaterialCatalogsToRawMaterialCatalog : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.RawMaterialCatalogs", newName: "RawMaterialCatalog");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.RawMaterialCatalog", newName: "RawMaterialCatalogs");
        }
    }
}
