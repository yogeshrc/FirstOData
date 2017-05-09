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

        //[
        //    HttpPost,
        //    ODataRoute("Suppliers/Create")
        //]
        //public IHttpActionResult Create()
        //{
        //    return Ok<bool>(true);
        //}

    }
}