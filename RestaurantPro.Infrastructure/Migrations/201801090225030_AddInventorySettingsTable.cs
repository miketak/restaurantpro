namespace RestaurantPro.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddInventorySettingsTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.InventorySettings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Parameter = c.String(),
                        Value = c.Decimal(precision: 14, scale: 4),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.InventorySettings");
        }
    }
}
