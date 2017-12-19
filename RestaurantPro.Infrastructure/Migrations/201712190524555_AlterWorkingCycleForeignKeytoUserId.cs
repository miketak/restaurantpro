namespace RestaurantPro.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterWorkingCycleForeignKeytoUserId : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.WorkCycles", "User_Id", "dbo.Users");
            DropIndex("dbo.WorkCycles", new[] { "User_Id" });
            RenameColumn(table: "dbo.WorkCycles", name: "User_Id", newName: "UserId");
            AlterColumn("dbo.WorkCycles", "UserId", c => c.Int(nullable: false));
            CreateIndex("dbo.WorkCycles", "UserId");
            AddForeignKey("dbo.WorkCycles", "UserId", "dbo.Users", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WorkCycles", "UserId", "dbo.Users");
            DropIndex("dbo.WorkCycles", new[] { "UserId" });
            AlterColumn("dbo.WorkCycles", "UserId", c => c.Int());
            RenameColumn(table: "dbo.WorkCycles", name: "UserId", newName: "User_Id");
            CreateIndex("dbo.WorkCycles", "User_Id");
            AddForeignKey("dbo.WorkCycles", "User_Id", "dbo.Users", "Id");
        }
    }
}
