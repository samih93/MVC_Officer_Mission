namespace MVC_Officer_Mission.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CaptainUpdate4 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.OfficerRoleInMissions", "MissionId", "dbo.Missions");
            DropIndex("dbo.OfficerRoleInMissions", new[] { "MissionId" });
            AddColumn("dbo.OfficerRoleInMissions", "TournamentId", c => c.Int(nullable: false));
            CreateIndex("dbo.OfficerRoleInMissions", "TournamentId");
            AddForeignKey("dbo.OfficerRoleInMissions", "TournamentId", "dbo.Tournaments", "Id", cascadeDelete: true);
            DropColumn("dbo.OfficerRoleInMissions", "MissionId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.OfficerRoleInMissions", "MissionId", c => c.Int(nullable: false));
            DropForeignKey("dbo.OfficerRoleInMissions", "TournamentId", "dbo.Tournaments");
            DropIndex("dbo.OfficerRoleInMissions", new[] { "TournamentId" });
            DropColumn("dbo.OfficerRoleInMissions", "TournamentId");
            CreateIndex("dbo.OfficerRoleInMissions", "MissionId");
            AddForeignKey("dbo.OfficerRoleInMissions", "MissionId", "dbo.Missions", "Id", cascadeDelete: true);
        }
    }
}
