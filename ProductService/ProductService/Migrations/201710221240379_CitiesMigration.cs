namespace ProductService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CitiesMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Lookups",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Cities", "Lookup_Id", c => c.Guid());
            CreateIndex("dbo.Cities", "Lookup_Id");
            AddForeignKey("dbo.Cities", "Lookup_Id", "dbo.Lookups", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Cities", "Lookup_Id", "dbo.Lookups");
            DropIndex("dbo.Cities", new[] { "Lookup_Id" });
            DropColumn("dbo.Cities", "Lookup_Id");
            DropTable("dbo.Lookups");
        }
    }
}
