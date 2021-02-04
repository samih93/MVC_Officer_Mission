namespace MVC_Officer_Mission.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMissionNameTolog : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MissionLogs", "mission_name", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.MissionLogs", "mission_name");
        }
    }
}
