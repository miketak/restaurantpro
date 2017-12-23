namespace RestaurantPro.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CorrectMoveDataNullableError : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.WorkCycleLines", "WasMoved", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.WorkCycleLines", "WasMoved", c => c.Boolean());
        }
    }
}
