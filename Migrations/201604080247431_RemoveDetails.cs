namespace DeviceMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveDetails : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Softwares", "Details");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Softwares", "Details", c => c.String(nullable: false, maxLength: 100));
        }
    }
}
