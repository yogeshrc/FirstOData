using ProductService.Models;
using System;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;

namespace ProductService
{
    internal class ODataConfig
    {
        public static void RegisterODataModel(HttpConfiguration config)
        {
            ODataModelBuilder builder = new ODataConventionModelBuilder();
            RegisterEntities(builder);
            RegisterFunctions(builder);
            RegisterActions(builder);
            config.MapODataServiceRoute(routeName: "ODataRoute", routePrefix: null, model: builder.GetEdmModel());
            config.MaxTop(null).OrderBy().Filter();
        }

        private static void RegisterActions(ODataModelBuilder builder)
        {
            //Bound action on a single Product
            var applyVatAction = builder.EntityType<Product>()
                .Action("ApplyVAT")
                .Returns<int>();
            applyVatAction.Parameter<int>("vat");
            applyVatAction.EntityParameter<City>("manufacturedIn");
            applyVatAction.CollectionEntityParameter<City>("availableCities");

            //Bound action on Suppliers collection
            builder.EntityType<Supplier>().Collection
                .Action("BulkInsert")
                .ReturnsCollectionFromEntitySet<Supplier>("Suppliers")
                .CollectionParameter<string>("suppliers");
        }

        private static void RegisterFunctions(ODataModelBuilder builder)
        {
            //Bound function on a single Product
            builder.EntityType<Product>()
                .Function("IsWithinBudget")
                .Returns<bool>()
                .Parameter<int>("budget");
        }

        private static void RegisterEntities(ODataModelBuilder builder)
        {
            builder.EntitySet<Product>("Products");
            builder.EntitySet<Supplier>("Suppliers");
            builder.EntitySet<City>("Cities");
        }


    }
}