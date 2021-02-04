namespace MVC_Officer_Mission.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OfficerProfileUpdate7 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Officers", "Mail", c => c.String());
            DropColumn("dbo.Officers", "Email");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Officers", "Email", c => c.String());
            DropColumn("dbo.Officers", "Mail");
        }
    }
}
