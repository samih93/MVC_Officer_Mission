namespace MVC_Officer_Mission.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OfficerProfileUpdate16 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Departments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Officers", "DepartmentId", c => c.Int());
            CreateIndex("dbo.Officers", "DepartmentId");
            AddForeignKey("dbo.Officers", "DepartmentId", "dbo.Departments", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Officers", "DepartmentId", "dbo.Departments");
            DropIndex("dbo.Officers", new[] { "DepartmentId" });
            DropColumn("dbo.Officers", "DepartmentId");
            DropTable("dbo.Departments");
        }
    }
}
