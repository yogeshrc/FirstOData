using ProductService.Models;
using System.Linq;
using System.Web.Http;
using System.Web.OData;

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
    }
}