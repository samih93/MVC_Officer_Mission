namespace MVC_Officer_Mission.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeMissionModelFromLog : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.MissionLogs", "MissionId", "dbo.Missions");
            DropIndex("dbo.MissionLogs", new[] { "MissionId" });
            AddColumn("dbo.MissionLogs", "Action_Id", c => c.Int(nullable: false));
            DropColumn("dbo.MissionLogs", "IsCreated");
        }
        
        public override void Down()
        {
            AddColumn("dbo.MissionLogs", "IsCreated", c => c.Boolean(nullable: false));
            DropColumn("dbo.MissionLogs", "Action_Id");
            CreateIndex("dbo.MissionLogs", "MissionId");
            AddForeignKey("dbo.MissionLogs", "MissionId", "dbo.Missions", "Id", cascadeDelete: true);
        }
    }
}
