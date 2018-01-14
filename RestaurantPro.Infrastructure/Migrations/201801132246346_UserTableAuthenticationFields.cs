namespace RestaurantPro.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserTableAuthenticationFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "PasswordHash", c => c.Binary());
            AddColumn("dbo.Users", "SaltHash", c => c.Binary());
            DropColumn("dbo.Users", "Password");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "Password", c => c.String());
            DropColumn("dbo.Users", "SaltHash");
            DropColumn("dbo.Users", "PasswordHash");
        }
    }
}
