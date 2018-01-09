namespace RestaurantPro.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRawMaterialActiveBit : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RawMaterials", "Active", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.RawMaterials", "Active");
        }
    }
}
