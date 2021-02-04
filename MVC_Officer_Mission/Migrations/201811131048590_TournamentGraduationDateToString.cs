namespace MVC_Officer_Mission.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TournamentGraduationDateToString : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tournaments", "GraduationDate", c => c.String());
            DropColumn("dbo.Tournaments", "GraduationTime");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Tournaments", "GraduationTime", c => c.DateTime(nullable: false));
            DropColumn("dbo.Tournaments", "GraduationDate");
        }
    }
}
