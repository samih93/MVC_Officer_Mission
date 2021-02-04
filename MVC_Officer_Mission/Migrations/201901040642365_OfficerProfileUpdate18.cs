namespace MVC_Officer_Mission.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OfficerProfileUpdate18 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Officers", "Address", c => c.String());
            AddColumn("dbo.Officers", "Email", c => c.String());
            DropColumn("dbo.Officers", "Address1");
            DropColumn("dbo.Officers", "Mail");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Officers", "Mail", c => c.String());
            AddColumn("dbo.Officers", "Address1", c => c.String());
            DropColumn("dbo.Officers", "Email");
            DropColumn("dbo.Officers", "Address");
        }
    }
}
