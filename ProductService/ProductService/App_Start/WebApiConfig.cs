using ProductService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;

namespace ProductService
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            ODataModelBuilder builder = new ODataConventionModelBuilder();
            builder.EntitySet<Product>("Products");
            builder.EntitySet<Supplier>("Suppliers");
            builder.EntitySet<City>("Manufacturers");

            //Bound function on Product
            builder.EntityType<Product>()
                .Function("IsWithinBudget")
                .Returns<bool>()
                .Parameter<int>("budget");

            //Bound action on Product
            var applyVatAction = builder.EntityType<Product>()
                .Action("ApplyVAT")
                .Returns<int>();
            applyVatAction.Parameter<int>("vat");
            applyVatAction.EntityParameter<City>("manufacturedIn");
            applyVatAction.CollectionEntityParameter<City>("availableCities");

            //Bound action on Supplier
            builder.EntityType<Supplier>()
                .Action("BulkInsert")
                .Returns<bool>()
                .Parameter<string>("products");

            config.MapODataServiceRoute(routeName: "ODataRoute", routePrefix: null, model: builder.GetEdmModel());
            config.MaxTop(null).OrderBy().Filter();
        }
    }
}
