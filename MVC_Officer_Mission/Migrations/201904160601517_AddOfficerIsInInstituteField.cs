namespace MVC_Officer_Mission.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOfficerIsInInstituteField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Officers", "IsInInstitute", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Officers", "IsInInstitute");
        }
    }
}
