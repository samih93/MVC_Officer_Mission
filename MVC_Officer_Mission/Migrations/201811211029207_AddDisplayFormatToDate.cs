namespace MVC_Officer_Mission.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDisplayFormatToDate : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Missions", "From", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Missions", "To", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Tournaments", "GraduationDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Tournaments", "GraduationDate", c => c.String());
            AlterColumn("dbo.Missions", "To", c => c.String(nullable: false));
            AlterColumn("dbo.Missions", "From", c => c.String(nullable: false));
        }
    }
}
