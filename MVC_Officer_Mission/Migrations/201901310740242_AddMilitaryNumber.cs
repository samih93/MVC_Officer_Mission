namespace MVC_Officer_Mission.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMilitaryNumber : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Officers", "MilitaryNumber", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Officers", "MilitaryNumber");
        }
    }
}
