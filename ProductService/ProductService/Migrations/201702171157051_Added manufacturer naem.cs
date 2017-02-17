namespace ProductService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addedmanufacturernaem : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "Manufacturer", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "Manufacturer");
        }
    }
}
