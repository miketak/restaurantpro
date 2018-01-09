namespace RestaurantPro.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLocationActiveBit : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Locations", "Active", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Locations", "Active");
        }
    }
}
