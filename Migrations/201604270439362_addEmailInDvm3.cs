namespace DeviceMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addEmailInDvm3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "DeviceToUser_DeviceID", c => c.Int());
            AddColumn("dbo.AspNetUsers", "DeviceToUser_UserID", c => c.String(maxLength: 128));
            CreateIndex("dbo.AspNetUsers", new[] { "DeviceToUser_DeviceID", "DeviceToUser_UserID" });
            AddForeignKey("dbo.AspNetUsers", new[] { "DeviceToUser_DeviceID", "DeviceToUser_UserID" }, "dbo.DeviceToUsers", new[] { "DeviceID", "UserID" });
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", new[] { "DeviceToUser_DeviceID", "DeviceToUser_UserID" }, "dbo.DeviceToUsers");
            DropIndex("dbo.AspNetUsers", new[] { "DeviceToUser_DeviceID", "DeviceToUser_UserID" });
            DropColumn("dbo.AspNetUsers", "DeviceToUser_UserID");
            DropColumn("dbo.AspNetUsers", "DeviceToUser_DeviceID");
        }
    }
}
