using ProductService.Models;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Routing;

namespace ProductService.Controllers
{
    public class SuppliersController : ODataController
    {
        private ProductsContext _context = new ProductsContext();

        [EnableQuery]
        public IQueryable<Supplier> Get()
        {
            return _context.Suppliers;
        }

        private const string ProductsParameterNotPassed = "'products' not found in JSON request body. Expected { \"products\": \"<product json object collection>\"}";
        [
            HttpPost,
            ODataRoute("Suppliers({key})/Default.BulkInsert")
        ]
        public IHttpActionResult BulkInsert([FromODataUri] int key, ODataActionParameters parameter)
        {
            object requestBody;
            if (!parameter.TryGetValue("products", out requestBody)) return BadRequest(ProductsParameterNotPassed);

            string jsonString = parameter["products"] as string;
            if (jsonString == null) return BadRequest(ProductsParameterNotPassed);

            var param = Newtonsoft.Json.JsonConvert.DeserializeObject(jsonString);

            return Ok(true);
        }

    }
}