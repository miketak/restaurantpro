namespace RestaurantPro.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPoStatusTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PoStatus",
                c => new
                    {
                        Status = c.String(nullable: false, maxLength: 15),
                    })
                .PrimaryKey(t => t.Status);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.PoStatus");
        }
    }
}
