namespace RestaurantPro.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddActiveBitToSupplier : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Suppliers", "Active", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Suppliers", "Active");
        }
    }
}
