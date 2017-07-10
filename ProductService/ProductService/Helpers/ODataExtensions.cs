using System.Web.OData;

namespace ProductService.Helpers
{
    public static class ODataExtensions
    {
        public static T Find<T>(this ODataActionParameters parameters, string parameterName)
        {
            object output;
            if (!parameters.TryGetValue(parameterName, out output))
                return default(T);
            return (T)output;
        }
    }
}