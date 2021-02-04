namespace MVC_Officer_Mission.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OfficerProfileUpdate3 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SpecificationOfficers", "Specification_Id", "dbo.Specifications");
            DropForeignKey("dbo.SpecificationOfficers", "Officer_Id", "dbo.Officers");
            DropIndex("dbo.SpecificationOfficers", new[] { "Specification_Id" });
            DropIndex("dbo.SpecificationOfficers", new[] { "Officer_Id" });
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
            
            CreateIndex("dbo.SpecificationOfficers", "Officer_Id");
            CreateIndex("dbo.SpecificationOfficers", "Specification_Id");
            AddForeignKey("dbo.SpecificationOfficers", "Officer_Id", "dbo.Officers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.SpecificationOfficers", "Specification_Id", "dbo.Specifications", "Id", cascadeDelete: true);
        }
    }
}
