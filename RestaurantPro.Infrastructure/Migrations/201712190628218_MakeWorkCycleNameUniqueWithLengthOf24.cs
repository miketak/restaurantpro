namespace RestaurantPro.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MakeWorkCycleNameUniqueWithLengthOf24 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.WorkCycles", "IX_WorkCycleName");
            AlterColumn("dbo.WorkCycles", "Name", c => c.String(nullable: false, maxLength: 24));
            CreateIndex("dbo.WorkCycles", "Name", unique: true, name: "IX_WorkCycleName");
        }
        
        public override void Down()
        {
            DropIndex("dbo.WorkCycles", "IX_WorkCycleName");
            AlterColumn("dbo.WorkCycles", "Name", c => c.String(nullable: false, maxLength: 10));
            CreateIndex("dbo.WorkCycles", "Name", unique: true, name: "IX_WorkCycleName");
        }
    }
}
