namespace MVC_Officer_Mission.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NonRequiredTournamentID : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Missions", "TournamentID", "dbo.Tournaments");
            DropIndex("dbo.Missions", new[] { "TournamentID" });
            AlterColumn("dbo.Missions", "TournamentID", c => c.Int());
            CreateIndex("dbo.Missions", "TournamentID");
            AddForeignKey("dbo.Missions", "TournamentID", "dbo.Tournaments", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Missions", "TournamentID", "dbo.Tournaments");
            DropIndex("dbo.Missions", new[] { "TournamentID" });
            AlterColumn("dbo.Missions", "TournamentID", c => c.Int(nullable: false));
            CreateIndex("dbo.Missions", "TournamentID");
            AddForeignKey("dbo.Missions", "TournamentID", "dbo.Tournaments", "Id", cascadeDelete: true);
        }
    }
}
