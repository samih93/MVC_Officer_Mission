namespace MVC_Officer_Mission.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OfficerProfileUpdate9 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Officers", "DepartmentId", "dbo.Departments");
            DropForeignKey("dbo.ExternalTournaments", "OfficerId", "dbo.Officers");
            DropForeignKey("dbo.SpecificationOfficers", "Specification_Id", "dbo.Specifications");
            DropForeignKey("dbo.SpecificationOfficers", "Officer_Id", "dbo.Officers");
            DropIndex("dbo.Officers", new[] { "DepartmentId" });
            DropIndex("dbo.ExternalTournaments", new[] { "OfficerId" });
            DropIndex("dbo.SpecificationOfficers", new[] { "Specification_Id" });
            DropIndex("dbo.SpecificationOfficers", new[] { "Officer_Id" });
            AddColumn("dbo.Officers", "Specification_Id", c => c.Int());
            CreateIndex("dbo.Officers", "Specification_Id");
            AddForeignKey("dbo.Officers", "Specification_Id", "dbo.Specifications", "Id");
            DropColumn("dbo.Officers", "FatherName");
            DropColumn("dbo.Officers", "MotherFullName");
            DropColumn("dbo.Officers", "BirthDate");
            DropColumn("dbo.Officers", "PhoneNumber");
            DropColumn("dbo.Officers", "Address1");
            DropColumn("dbo.Officers", "Mail");
            DropColumn("dbo.Officers", "CollegeCertificates");
            DropColumn("dbo.Officers", "DepartmentId");
            DropColumn("dbo.Officers", "IsHeadOfDepartment");
            DropColumn("dbo.Officers", "ProfileImage");
            DropTable("dbo.Departments");
            DropTable("dbo.ExternalTournaments");
            DropTable("dbo.SpecificationOfficers");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.SpecificationOfficers",
                c => new
                    {
                        Specification_Id = c.Int(nullable: false),
                        Officer_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Specification_Id, t.Officer_Id });
            
            CreateTable(
                "dbo.ExternalTournaments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        OfficerId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Departments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Officers", "ProfileImage", c => c.String());
            AddColumn("dbo.Officers", "IsHeadOfDepartment", c => c.Boolean(nullable: false));
            AddColumn("dbo.Officers", "DepartmentId", c => c.Int());
            AddColumn("dbo.Officers", "CollegeCertificates", c => c.String());
            AddColumn("dbo.Officers", "Mail", c => c.String());
            AddColumn("dbo.Officers", "Address1", c => c.String());
            AddColumn("dbo.Officers", "PhoneNumber", c => c.String());
            AddColumn("dbo.Officers", "BirthDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Officers", "MotherFullName", c => c.String());
            AddColumn("dbo.Officers", "FatherName", c => c.String());
            DropForeignKey("dbo.Officers", "Specification_Id", "dbo.Specifications");
            DropIndex("dbo.Officers", new[] { "Specification_Id" });
            DropColumn("dbo.Officers", "Specification_Id");
            CreateIndex("dbo.SpecificationOfficers", "Officer_Id");
            CreateIndex("dbo.SpecificationOfficers", "Specification_Id");
            CreateIndex("dbo.ExternalTournaments", "OfficerId");
            CreateIndex("dbo.Officers", "DepartmentId");
            AddForeignKey("dbo.SpecificationOfficers", "Officer_Id", "dbo.Officers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.SpecificationOfficers", "Specification_Id", "dbo.Specifications", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ExternalTournaments", "OfficerId", "dbo.Officers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Officers", "DepartmentId", "dbo.Departments", "Id");
        }
    }
}
