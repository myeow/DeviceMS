namespace DeviceMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeUserIDfield : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.DeviceToUsers", "UserID", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.DeviceToUsers", "UserID", c => c.Int(nullable: false));
        }
    }
}
