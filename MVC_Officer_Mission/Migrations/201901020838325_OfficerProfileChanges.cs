namespace MVC_Officer_Mission.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OfficerProfileChanges : DbMigration
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
                "dbo.Specifications",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
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
            
            AddColumn("dbo.Officers", "FatherName", c => c.String());
            AddColumn("dbo.Officers", "MotherFullName", c => c.String());
            AddColumn("dbo.Officers", "BirthDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Officers", "PhoneNumber", c => c.String());
            AddColumn("dbo.Officers", "Address", c => c.String());
            AddColumn("dbo.Officers", "Email", c => c.String());
            AddColumn("dbo.Officers", "Certificates", c => c.String());
            DropColumn("dbo.Officers", "WorkLocation");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Officers", "WorkLocation", c => c.String());
            DropForeignKey("dbo.SpecificationOfficers", "Officer_Id", "dbo.Officers");
            DropForeignKey("dbo.SpecificationOfficers", "Specification_Id", "dbo.Specifications");
            DropForeignKey("dbo.ExternalTournaments", "OfficerId", "dbo.Officers");
            DropIndex("dbo.SpecificationOfficers", new[] { "Officer_Id" });
            DropIndex("dbo.SpecificationOfficers", new[] { "Specification_Id" });
            DropIndex("dbo.ExternalTournaments", new[] { "OfficerId" });
            DropColumn("dbo.Officers", "Certificates");
            DropColumn("dbo.Officers", "Email");
            DropColumn("dbo.Officers", "Address");
            DropColumn("dbo.Officers", "PhoneNumber");
            DropColumn("dbo.Officers", "BirthDate");
            DropColumn("dbo.Officers", "MotherFullName");
            DropColumn("dbo.Officers", "FatherName");
            DropTable("dbo.SpecificationOfficers");
            DropTable("dbo.Specifications");
            DropTable("dbo.ExternalTournaments");
        }
    }
}
