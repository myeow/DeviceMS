namespace DeviceMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeDvUfields : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.DeviceToUsers", "DeviceID", "dbo.Devices");
            DropIndex("dbo.DeviceToUsers", new[] { "DeviceID" });
            DropPrimaryKey("dbo.DeviceToUsers");
            AddColumn("dbo.DeviceToUsers", "DateCreated", c => c.DateTime(nullable: false));
            AlterColumn("dbo.DeviceToUsers", "UserID", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.DeviceToUsers", new[] { "DeviceID", "UserID" });
            DropColumn("dbo.DeviceToUsers", "DeviceToUserID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DeviceToUsers", "DeviceToUserID", c => c.Int(nullable: false, identity: true));
            DropPrimaryKey("dbo.DeviceToUsers");
            AlterColumn("dbo.DeviceToUsers", "UserID", c => c.String());
            DropColumn("dbo.DeviceToUsers", "DateCreated");
            AddPrimaryKey("dbo.DeviceToUsers", "DeviceToUserID");
            CreateIndex("dbo.DeviceToUsers", "DeviceID");
            AddForeignKey("dbo.DeviceToUsers", "DeviceID", "dbo.Devices", "DeviceId", cascadeDelete: true);
        }
    }
}
