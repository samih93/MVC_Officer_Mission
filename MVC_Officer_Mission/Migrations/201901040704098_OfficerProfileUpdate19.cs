namespace MVC_Officer_Mission.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OfficerProfileUpdate19 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Officers", "SpecificationId", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Officers", "SpecificationId");
        }
    }
}
