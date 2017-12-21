namespace RestaurantPro.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveMeasureFromRawMaterial : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.RawMaterials", "Measure");
        }
        
        public override void Down()
        {
            AddColumn("dbo.RawMaterials", "Measure", c => c.String());
        }
    }
}
