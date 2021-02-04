namespace MVC_Officer_Mission.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMissionLog : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MissionLogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        MissionId = c.Int(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsCreated = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Missions", t => t.MissionId, cascadeDelete: true)
                .Index(t => t.MissionId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MissionLogs", "MissionId", "dbo.Missions");
            DropIndex("dbo.MissionLogs", new[] { "MissionId" });
            DropTable("dbo.MissionLogs");
        }
    }
}
