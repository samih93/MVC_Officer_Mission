namespace MVC_Officer_Mission.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OfficerUpdateprofile20 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Officers", "SpecificationId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Officers", "SpecificationId", c => c.Int());
        }
    }
}
