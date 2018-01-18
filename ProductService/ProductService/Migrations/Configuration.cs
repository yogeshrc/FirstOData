namespace ProductService.Migrations
{
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ProductService.Models.ProductsContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ProductService.Models.ProductsContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            context.Suppliers.AddOrUpdate<Supplier>(
                new Supplier { Id = 1, Name = "OReilly" },
                new Supplier { Id = 2, Name = "Pearson" },
                new Supplier { Id = 3, Name = "Sams" });

            context.Products.AddOrUpdate<Product>(
                new Product { Id = 1, Name = "Domain Driven Design", SupplierId = 2, Category = "Design", Price = 700 },
                new Product { Id = 2, Name = "UML in 24 hours", SupplierId = 3, Category = "Design", Price = 195 },
                new Product { Id = 3, Name = "iPhone Game Development", SupplierId = 1, Category = "Programming", Price = 300 },
                new Product { Id = 4, Name = "AI for Game Developers", SupplierId = 1, Category = "Programming", Price = 200 }
                );

            context.Cities.AddOrUpdate<City>(
                new City { Id = 1, Name = "Pune" },
                new City { Id = 2, Name = "Mumbai" },
                new City { Id = 3, Name = "Bedford" }
                );

            context.SaveChanges();
        }
    }
}
