namespace DeviceMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeUniqueKey : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AspNetUsers", new[] { "DeviceToUser_DeviceID", "DeviceToUser_UserID" }, "dbo.DeviceToUsers");
            DropForeignKey("dbo.AspNetUsers", "DeviceToUser_DeviceToUserId", "dbo.DeviceToUsers");
            DropIndex("dbo.AspNetUsers", new[] { "DeviceToUser_DeviceID", "DeviceToUser_UserID" });
            RenameColumn(table: "dbo.AspNetUsers", name: "DeviceToUser_DeviceID", newName: "DeviceToUser_DeviceToUserId");
            DropPrimaryKey("dbo.DeviceToUsers");
            AddColumn("dbo.DeviceToUsers", "DeviceToUserId", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.DeviceToUsers", "UserID", c => c.String());
            AddPrimaryKey("dbo.DeviceToUsers", "DeviceToUserId");
            CreateIndex("dbo.AspNetUsers", "DeviceToUser_DeviceToUserId");
            AddForeignKey("dbo.AspNetUsers", "DeviceToUser_DeviceToUserId", "dbo.DeviceToUsers", "DeviceToUserId");
            DropColumn("dbo.AspNetUsers", "DeviceToUser_UserID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "DeviceToUser_UserID", c => c.String(maxLength: 128));
            DropForeignKey("dbo.AspNetUsers", "DeviceToUser_DeviceToUserId", "dbo.DeviceToUsers");
            DropIndex("dbo.AspNetUsers", new[] { "DeviceToUser_DeviceToUserId" });
            DropPrimaryKey("dbo.DeviceToUsers");
            AlterColumn("dbo.DeviceToUsers", "UserID", c => c.String(nullable: false, maxLength: 128));
            DropColumn("dbo.DeviceToUsers", "DeviceToUserId");
            AddPrimaryKey("dbo.DeviceToUsers", new[] { "DeviceID", "UserID" });
            RenameColumn(table: "dbo.AspNetUsers", name: "DeviceToUser_DeviceToUserId", newName: "DeviceToUser_DeviceID");
            CreateIndex("dbo.AspNetUsers", new[] { "DeviceToUser_DeviceID", "DeviceToUser_UserID" });
            AddForeignKey("dbo.AspNetUsers", "DeviceToUser_DeviceToUserId", "dbo.DeviceToUsers", "DeviceToUserId");
            AddForeignKey("dbo.AspNetUsers", new[] { "DeviceToUser_DeviceID", "DeviceToUser_UserID" }, "dbo.DeviceToUsers", new[] { "DeviceID", "UserID" });
        }
    }
}
