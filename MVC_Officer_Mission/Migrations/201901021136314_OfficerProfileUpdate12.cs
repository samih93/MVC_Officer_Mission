namespace MVC_Officer_Mission.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OfficerProfileUpdate12 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Officers", "CollegeCertificates");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Officers", "CollegeCertificates", c => c.String());
        }
    }
}
