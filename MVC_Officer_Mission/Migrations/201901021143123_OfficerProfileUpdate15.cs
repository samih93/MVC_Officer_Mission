namespace MVC_Officer_Mission.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OfficerProfileUpdate15 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Officers", "Specification_Id", "dbo.Specifications");
            DropIndex("dbo.Officers", new[] { "Specification_Id" });
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
            
            AddColumn("dbo.Officers", "CollegeCertificates", c => c.String());
            AddColumn("dbo.Officers", "IsHeadOfDepartment", c => c.Boolean(nullable: false));
            AddColumn("dbo.Officers", "ProfileImage", c => c.String());
            DropColumn("dbo.Officers", "Specification_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Officers", "Specification_Id", c => c.Int());
            DropForeignKey("dbo.SpecificationOfficers", "Officer_Id", "dbo.Officers");
            DropForeignKey("dbo.SpecificationOfficers", "Specification_Id", "dbo.Specifications");
            DropForeignKey("dbo.ExternalTournaments", "OfficerId", "dbo.Officers");
            DropIndex("dbo.SpecificationOfficers", new[] { "Officer_Id" });
            DropIndex("dbo.SpecificationOfficers", new[] { "Specification_Id" });
            DropIndex("dbo.ExternalTournaments", new[] { "OfficerId" });
            DropColumn("dbo.Officers", "ProfileImage");
            DropColumn("dbo.Officers", "IsHeadOfDepartment");
            DropColumn("dbo.Officers", "CollegeCertificates");
            DropTable("dbo.SpecificationOfficers");
            DropTable("dbo.ExternalTournaments");
            CreateIndex("dbo.Officers", "Specification_Id");
            AddForeignKey("dbo.Officers", "Specification_Id", "dbo.Specifications", "Id");
        }
    }
}
