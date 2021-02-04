namespace MVC_Officer_Mission.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OfficerProfileUpdate6 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ExternalTournaments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        OfficerId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Officers", t => t.OfficerId, cascadeDelete: true)
                .Index(t => t.OfficerId);
            
            CreateTable(
                "dbo.SpecificationOfficers",
                c => new
                    {
                        Specification_Id = c.Int(nullable: false),
                        Officer_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Specification_Id, t.Officer_Id })
                .ForeignKey("dbo.Specifications", t => t.Specification_Id, cascadeDelete: true)
                .ForeignKey("dbo.Officers", t => t.Officer_Id, cascadeDelete: true)
                .Index(t => t.Specification_Id)
                .Index(t => t.Officer_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SpecificationOfficers", "Officer_Id", "dbo.Officers");
            DropForeignKey("dbo.SpecificationOfficers", "Specification_Id", "dbo.Specifications");
            DropForeignKey("dbo.ExternalTournaments", "OfficerId", "dbo.Officers");
            DropIndex("dbo.SpecificationOfficers", new[] { "Officer_Id" });
            DropIndex("dbo.SpecificationOfficers", new[] { "Specification_Id" });
            DropIndex("dbo.ExternalTournaments", new[] { "OfficerId" });
            DropTable("dbo.SpecificationOfficers");
            DropTable("dbo.ExternalTournaments");
        }
    }
}
