namespace MVC_Officer_Mission.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TournamentGraduationDateNotRequired : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Tournaments", "GraduationDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Tournaments", "GraduationDate", c => c.DateTime(nullable: false));
        }
    }
}
