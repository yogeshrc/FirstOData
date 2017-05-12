using ProductService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Routing;

namespace ProductService.Controllers
{
    public class ProductsController: ODataController
    {
        private ProductsContext _database = new ProductsContext();

        private bool ProductExists(int productId)
        {
            return _database.Products.Any(product => product.Id == productId);
        }

        protected override void Dispose(bool disposing)
        {
            _database.Dispose();
            base.Dispose(disposing);
        }

        [EnableQuery]
        public IQueryable<Product> Get()
        {
            return _database.Products;
        }

        [EnableQuery]
        public SingleResult<Product> Get([FromODataUri] int key)
        {
            IQueryable<Product> result = _database.Products.Where(product => product.Id == key);
            return SingleResult.Create(result);
        }

        public async Task<IHttpActionResult> Post(Product product)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            _database.Products.Add(product);
            await _database.SaveChangesAsync();
            return Created(product);
        }

        [HttpGet]
        [ODataRoute("Products({key})/Default.IsWithinBudget(budget={budget})")]
        public bool IsWithinBudget([FromODataUri] int key, [FromODataUri] int budget)
        {
            return true;
        }

        [HttpPost]
        [ODataRoute("Products({key})/Default.ApplyVAT")]
        public async Task<IHttpActionResult> ApplyVAT([FromODataUri] int key, ODataActionParameters parameters)
        {
            if (parameters == null) return BadRequest("Undefined parameter request body");

            Product product = await Task.Run(() => _database.Products.SingleOrDefault(p => p.Id == key));
            if (product == null) return BadRequest("Product does not exist!!!");
            
            int vatPercent = parameters.Find<int>("vat");
            if (vatPercent == 0) return Ok(product.Price); //No change in price if VAT not defined

            //No VAT in the manufacturing city
            City manufacturingCity = parameters.Find<City>("manufacturedIn");
            if (manufacturingCity == null) return BadRequest("Incorrect data format or missing manufacturing city");

            IEnumerable<City> availableCities = parameters.Find<IEnumerable<City>>("availableCities");
            if (availableCities == null) return BadRequest("Incorrect data format or missing available cities");
            if (availableCities.SingleOrDefault(city => city.Id == manufacturingCity.Id) != null) return Ok(product.Price);

            //Update VATted price for the Product
            product.Price = product.Price + (product.Price * vatPercent) / 100;
            return Ok(product.Price);
        }

        private T FindParameter<T>(ODataActionParameters parameters, string parameterName)
        {
            object output;
            if (!parameters.TryGetValue(parameterName, out output)) return default(T);
            return (T)output;
        }
    }

    static class ODataExtensions
    {
        public static T Find<T>(this ODataActionParameters parameters, string parameterName)
        {
            object output;
            if (!parameters.TryGetValue(parameterName, out output)) return default(T);
            return (T)output;
        }
    }
}