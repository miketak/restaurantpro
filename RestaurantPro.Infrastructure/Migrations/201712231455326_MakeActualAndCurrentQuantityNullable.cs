namespace RestaurantPro.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MakeActualAndCurrentQuantityNullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.WorkCycleLines", "ActualQuantity", c => c.Single());
            AlterColumn("dbo.WorkCycleLines", "CurrentQuantity", c => c.Single());
            AlterColumn("dbo.WorkCycleLines", "WasMoved", c => c.Boolean());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.WorkCycleLines", "WasMoved", c => c.Boolean(nullable: false));
            AlterColumn("dbo.WorkCycleLines", "CurrentQuantity", c => c.Single(nullable: false));
            AlterColumn("dbo.WorkCycleLines", "ActualQuantity", c => c.Single(nullable: false));
        }
    }
}
