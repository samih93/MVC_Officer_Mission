namespace MVC_Officer_Mission.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addrequiredattribut : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Missions", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Missions", "From", c => c.String(nullable: false));
            AlterColumn("dbo.Missions", "To", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Missions", "To", c => c.String());
            AlterColumn("dbo.Missions", "From", c => c.String());
            AlterColumn("dbo.Missions", "Name", c => c.String());
        }
    }
}
