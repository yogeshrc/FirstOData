using ProductService.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Routing;
using ProductService.Helpers;
using System.Net.Http;

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

        private const string SuppliersParameterNotPassed = "'suppliers' not found in JSON request body. " +
                                                    "Expected { \"suppliers\": \"<suppliers list>\"}";
        [
            HttpPost,
            ODataRoute("Suppliers/Default.BulkInsert")
        ]
        public IHttpActionResult BulkInsert(ODataActionParameters parameter)
        {
            var suppliersParam = parameter.Find<IEnumerable<string>>("suppliers");
            if (suppliersParam == null) return BadRequest(SuppliersParameterNotPassed);

            IEnumerable<Supplier> suppliers = suppliersParam.Select(s => new Supplier { Name = s });
            _context.Suppliers.AddRange(suppliers);
            _context.SaveChanges();

            var newSuppliers = new List<Supplier>();
            foreach (var supplier in _context.Suppliers)
                if (suppliers.SingleOrDefault(s => s.Name == supplier.Name) != null)
                    newSuppliers.Add(supplier);

            return ResponseMessage(Request.CreateResponse(System.Net.HttpStatusCode.Created, newSuppliers));
        }

    }
}