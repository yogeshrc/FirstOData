using System.Web.Http;

namespace ProductService
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // Web API routes
            ODataConfig.RegisterODataModel(config);

        }
    }
}
