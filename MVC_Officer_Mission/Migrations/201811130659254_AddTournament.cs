namespace MVC_Officer_Mission.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTournament : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tournaments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Department = c.String(),
                        Room = c.String(),
                        GraduationTime = c.DateTime(nullable: false),
                        DocumentOrder = c.String(),
                        DocumentOrderFile = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Officers", "RankId", c => c.Int(nullable: false));
            AddColumn("dbo.Officers", "WorkLocation", c => c.String());
            AddColumn("dbo.Officers", "ProfileImage", c => c.String());
            AlterColumn("dbo.Missions", "Tournament_Id", c => c.Int());
            CreateIndex("dbo.Missions", "Tournament_Id");
            CreateIndex("dbo.Officers", "RankId");
            AddForeignKey("dbo.Officers", "RankId", "dbo.Ranks", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Missions", "Tournament_Id", "dbo.Tournaments", "Id");
            DropColumn("dbo.Officers", "Rank_id");
            DropColumn("dbo.Officers", "Work_Location");
            DropColumn("dbo.Officers", "Profile_image");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Officers", "Profile_image", c => c.String());
            AddColumn("dbo.Officers", "Work_Location", c => c.String());
            AddColumn("dbo.Officers", "Rank_id", c => c.Int(nullable: false));
            DropForeignKey("dbo.Missions", "Tournament_Id", "dbo.Tournaments");
            DropForeignKey("dbo.Officers", "RankId", "dbo.Ranks");
            DropIndex("dbo.Officers", new[] { "RankId" });
            DropIndex("dbo.Missions", new[] { "Tournament_Id" });
            AlterColumn("dbo.Missions", "Tournament_Id", c => c.Int(nullable: false));
            DropColumn("dbo.Officers", "ProfileImage");
            DropColumn("dbo.Officers", "WorkLocation");
            DropColumn("dbo.Officers", "RankId");
            DropTable("dbo.Tournaments");
        }
    }
}
