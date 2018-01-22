namespace RestaurantPro.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InventoryTransactionsTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.InventoryTransactions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        InventoryId = c.Int(nullable: false),
                        Quantity = c.Double(nullable: false),
                        CreatedBy = c.Int(nullable: false),
                        TransactionDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Inventory", t => t.InventoryId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.CreatedBy)
                .Index(t => t.InventoryId)
                .Index(t => t.CreatedBy);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.InventoryTransactions", "CreatedBy", "dbo.Users");
            DropForeignKey("dbo.InventoryTransactions", "InventoryId", "dbo.Inventory");
            DropIndex("dbo.InventoryTransactions", new[] { "CreatedBy" });
            DropIndex("dbo.InventoryTransactions", new[] { "InventoryId" });
            DropTable("dbo.InventoryTransactions");
        }
    }
}
