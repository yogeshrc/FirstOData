using ProductService.Models;
using System.Linq;
using System.Web.Http.OData.Query;
using System.Web.OData;
using System.Web.OData.Routing;

namespace ProductService.Controllers
{
    public class CitiesController : ODataController
    {
        //private ProductsContext _database = new ProductsContext();

        [EnableQuery]
        public IQueryable<City> Get(ODataQueryOptions query)
        {
            return new City[]
            {
                new City{Id = 1, Name = "Pune"},
                new City{Id = 2, Name = "Mumbai"}
            }.AsQueryable();
            //return _database.Cities;
        }
    }
}