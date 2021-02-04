namespace MVC_Officer_Mission.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OfficerProfileUpdate13 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Officers", "IsHeadOfDepartment");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Officers", "IsHeadOfDepartment", c => c.Boolean(nullable: false));
        }
    }
}
