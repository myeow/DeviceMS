namespace DeviceMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeSoftwareDateFormate : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Softwares", "DateCreated", c => c.DateTime());
            AlterColumn("dbo.Softwares", "DateModified", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Softwares", "DateModified", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Softwares", "DateCreated", c => c.DateTime(nullable: false));
        }
    }
}
