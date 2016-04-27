namespace DeviceMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changedateformat : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Devices", "DateCreated", c => c.DateTime());
            AlterColumn("dbo.Devices", "DateModified", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Devices", "DateModified", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Devices", "DateCreated", c => c.DateTime(nullable: false));
        }
    }
}
