namespace MVC_Officer_Mission.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OfficerProfileUpdate4 : DbMigration
    {
        public override void Up()
        {
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
            DropIndex("dbo.SpecificationOfficers", new[] { "Officer_Id" });
            DropIndex("dbo.SpecificationOfficers", new[] { "Specification_Id" });
            DropTable("dbo.SpecificationOfficers");
        }
    }
}
