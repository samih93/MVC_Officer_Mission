namespace MVC_Officer_Mission.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OfficerProfileUpdate8 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Officers", "Address1", c => c.String());
            AddColumn("dbo.Officers", "CollegeCertificates", c => c.String());
            DropColumn("dbo.Officers", "Address");
            DropColumn("dbo.Officers", "Certificates");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Officers", "Certificates", c => c.String());
            AddColumn("dbo.Officers", "Address", c => c.String());
            DropColumn("dbo.Officers", "CollegeCertificates");
            DropColumn("dbo.Officers", "Address1");
        }
    }
}
