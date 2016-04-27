namespace DeviceMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addEmailInDvm2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AspNetUsers", new[] { "DeviceToUser_DeviceID", "DeviceToUser_UserID" }, "dbo.DeviceToUsers");
            DropIndex("dbo.AspNetUsers", new[] { "DeviceToUser_DeviceID", "DeviceToUser_UserID" });
            DropColumn("dbo.AspNetUsers", "DeviceToUser_DeviceID");
            DropColumn("dbo.AspNetUsers", "DeviceToUser_UserID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "DeviceToUser_UserID", c => c.String(maxLength: 128));
            AddColumn("dbo.AspNetUsers", "DeviceToUser_DeviceID", c => c.Int());
            CreateIndex("dbo.AspNetUsers", new[] { "DeviceToUser_DeviceID", "DeviceToUser_UserID" });
            AddForeignKey("dbo.AspNetUsers", new[] { "DeviceToUser_DeviceID", "DeviceToUser_UserID" }, "dbo.DeviceToUsers", new[] { "DeviceID", "UserID" });
        }
    }
}
