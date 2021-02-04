namespace MVC_Officer_Mission.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateSpecifications : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Specifications", "Name", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Specifications", "Name", c => c.String());
        }
    }
}
