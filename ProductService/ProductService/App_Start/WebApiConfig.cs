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

            //Bound function on Product
            var isWithinBudgetFunc = builder.EntityType<Product>().Function("IsWithinBudget");
            isWithinBudgetFunc.Parameter<int>("budget");
            isWithinBudgetFunc.Returns<bool>();

            //Bound action on Supplier
            //builder.EntityType<Supplier>().Action("Create").Returns<bool>();

            config.MapODataServiceRoute(routeName: "ODataRoute", routePrefix: null, model: builder.GetEdmModel());
            config.MaxTop(null).OrderBy().Filter();
        }
    }
}
