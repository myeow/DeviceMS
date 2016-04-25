namespace DeviceMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeIdentityModels : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.DeviceToUsers", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.DeviceToUsers", new[] { "ApplicationUser_Id" });
            DropColumn("dbo.DeviceToUsers", "ApplicationUser_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DeviceToUsers", "ApplicationUser_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.DeviceToUsers", "ApplicationUser_Id");
            AddForeignKey("dbo.DeviceToUsers", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
