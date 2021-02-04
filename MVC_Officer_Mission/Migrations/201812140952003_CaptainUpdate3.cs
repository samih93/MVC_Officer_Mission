namespace MVC_Officer_Mission.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CaptainUpdate3 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.OfficerRoleInMissions", "OfficerRoleId", "dbo.OfficerRoles");
            DropIndex("dbo.OfficerRoleInMissions", new[] { "OfficerRoleId" });
            AddColumn("dbo.OfficerRoleInMissions", "OfficerRole", c => c.String());
            DropColumn("dbo.OfficerRoleInMissions", "OfficerRoleId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.OfficerRoleInMissions", "OfficerRoleId", c => c.Int(nullable: false));
            DropColumn("dbo.OfficerRoleInMissions", "OfficerRole");
            CreateIndex("dbo.OfficerRoleInMissions", "OfficerRoleId");
            AddForeignKey("dbo.OfficerRoleInMissions", "OfficerRoleId", "dbo.OfficerRoles", "Id", cascadeDelete: true);
        }
    }
}
