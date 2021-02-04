namespace MVC_Officer_Mission.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddForeignKeyMissions : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Missions", "Tournament_Id", "dbo.Tournaments");
            DropIndex("dbo.Missions", new[] { "Tournament_Id" });
            RenameColumn(table: "dbo.Missions", name: "Tournament_Id", newName: "TournamentID");
            AlterColumn("dbo.Missions", "TournamentID", c => c.Int(nullable: false));
            CreateIndex("dbo.Missions", "TournamentID");
            AddForeignKey("dbo.Missions", "TournamentID", "dbo.Tournaments", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Missions", "TournamentID", "dbo.Tournaments");
            DropIndex("dbo.Missions", new[] { "TournamentID" });
            AlterColumn("dbo.Missions", "TournamentID", c => c.Int());
            RenameColumn(table: "dbo.Missions", name: "TournamentID", newName: "Tournament_Id");
            CreateIndex("dbo.Missions", "Tournament_Id");
            AddForeignKey("dbo.Missions", "Tournament_Id", "dbo.Tournaments", "Id");
        }
    }
}
