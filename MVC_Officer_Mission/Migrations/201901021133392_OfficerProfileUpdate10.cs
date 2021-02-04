namespace MVC_Officer_Mission.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OfficerProfileUpdate10 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Officers", "FatherName", c => c.String());
            AddColumn("dbo.Officers", "MotherFullName", c => c.String());
            AddColumn("dbo.Officers", "BirthDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Officers", "PhoneNumber", c => c.String());
            AddColumn("dbo.Officers", "Address1", c => c.String());
            AddColumn("dbo.Officers", "Mail", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Officers", "Mail");
            DropColumn("dbo.Officers", "Address1");
            DropColumn("dbo.Officers", "PhoneNumber");
            DropColumn("dbo.Officers", "BirthDate");
            DropColumn("dbo.Officers", "MotherFullName");
            DropColumn("dbo.Officers", "FatherName");
        }
    }
}
