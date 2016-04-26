namespace DeviceMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class test : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Devices", "ApplicationUser_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.AspNetUsers", "DeviceToUser_DeviceID", c => c.Int());
            AddColumn("dbo.AspNetUsers", "DeviceToUser_UserID", c => c.String(maxLength: 128));
            CreateIndex("dbo.Devices", "ApplicationUser_Id");
            CreateIndex("dbo.DeviceToUsers", "DeviceID");
            CreateIndex("dbo.AspNetUsers", new[] { "DeviceToUser_DeviceID", "DeviceToUser_UserID" });
            AddForeignKey("dbo.Devices", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.AspNetUsers", new[] { "DeviceToUser_DeviceID", "DeviceToUser_UserID" }, "dbo.DeviceToUsers", new[] { "DeviceID", "UserID" });
            AddForeignKey("dbo.DeviceToUsers", "DeviceID", "dbo.Devices", "DeviceId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DeviceToUsers", "DeviceID", "dbo.Devices");
            DropForeignKey("dbo.AspNetUsers", new[] { "DeviceToUser_DeviceID", "DeviceToUser_UserID" }, "dbo.DeviceToUsers");
            DropForeignKey("dbo.Devices", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.AspNetUsers", new[] { "DeviceToUser_DeviceID", "DeviceToUser_UserID" });
            DropIndex("dbo.DeviceToUsers", new[] { "DeviceID" });
            DropIndex("dbo.Devices", new[] { "ApplicationUser_Id" });
            DropColumn("dbo.AspNetUsers", "DeviceToUser_UserID");
            DropColumn("dbo.AspNetUsers", "DeviceToUser_DeviceID");
            DropColumn("dbo.Devices", "ApplicationUser_Id");
        }
    }
}
