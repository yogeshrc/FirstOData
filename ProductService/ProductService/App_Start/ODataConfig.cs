using ProductService.Models;
using System.Web.Http;
using System.Web.OData.Batch;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;

namespace ProductService
{
    using ProductService.Models.SingletonExample;
    using System.Web.OData.Routing;
    using System.Web.OData.Routing.Conventions;

    internal class ODataConfig
    {
        public static void RegisterODataModel(HttpConfiguration config)
        {
            ODataBatchHandler odataBatchHandler = new DefaultODataBatchHandler(GlobalConfiguration.DefaultServer);

            ODataModelBuilder builder = new ODataConventionModelBuilder();
            RegisterEntities(builder);
            RegisterFunctions(builder);
            RegisterActions(builder);

            
            var conventions = ODataRoutingConventions.CreateDefault();
            //conventions.Insert(0, new CityCustomConvention());

            config.MapODataServiceRoute(
                routeName: "ODataRoute",
                routePrefix: "odata",
                model: builder.GetEdmModel(),
                pathHandler: new DefaultODataPathHandler(),
                routingConventions: conventions,
                batchHandler: odataBatchHandler
                );
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
            builder.Singleton<Lookup>("Lookups");

            //containment entities
            builder.EntitySet<Account>("Accounts");

            //singleton entities
            builder.EntitySet<Employee>("Employees");
            builder.Singleton<Company>(name: "Umbrella");   
        }
    }
}