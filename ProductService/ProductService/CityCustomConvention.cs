using System.Linq;
using System.Web.Http.Controllers;

namespace ProductService
{
    using System;
    using System.Net.Http;
    using System.Web.OData.Routing;
    using System.Web.OData.Routing.Conventions;

    public class CityCustomConvention : IODataRoutingConvention
    {
        public string SelectAction(ODataPath odataPath, HttpControllerContext controllerContext, ILookup<string, HttpActionDescriptor> actionMap)
        {
            if (IsValidLookupPath(odataPath))
                return "Get" + odataPath.NavigationSource.Name;
            return null;
        }

        private bool IsValidLookupPath(ODataPath odataPath)
        {
            if (odataPath.Segments.Count < 2) return false;
            return (odataPath.Segments[0].Identifier.Equals("Lookups"));
        }

        public string SelectController(ODataPath odataPath, HttpRequestMessage request)
        {
            return null;
        }
    }
}