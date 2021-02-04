namespace MVC_Officer_Mission.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OfficerProfileUpdate17 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.OfficerMissions", newName: "MissionOfficers");
            DropPrimaryKey("dbo.MissionOfficers");
            AddPrimaryKey("dbo.MissionOfficers", new[] { "Mission_Id", "Officer_Id" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.MissionOfficers");
            AddPrimaryKey("dbo.MissionOfficers", new[] { "Officer_Id", "Mission_Id" });
            RenameTable(name: "dbo.MissionOfficers", newName: "OfficerMissions");
        }
    }
}
