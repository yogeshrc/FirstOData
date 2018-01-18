using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;
using System.Web.Http.OData.Routing;
using ProductService.Models;
using Microsoft.Data.OData;

namespace ProductService.Controllers
{
    /*
    The WebApiConfig class may require additional changes to add a route for this controller. Merge these statements into the Register method of the WebApiConfig class as applicable. Note that OData URLs are case sensitive.

    using System.Web.Http.OData.Builder;
    using System.Web.Http.OData.Extensions;
    using ProductService.Models;
    ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
    builder.EntitySet<Category>("Categories");
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class CategoriesController : ODataController
    {
        private static ODataValidationSettings _validationSettings = new ODataValidationSettings();

        // GET: odata/Categories
        public IHttpActionResult GetCategories(ODataQueryOptions<Category> queryOptions)
        {
            // validate the query.
            try
            {
                queryOptions.Validate(_validationSettings);
            }
            catch (ODataException ex)
            {
                return BadRequest(ex.Message);
            }

            List<Category> categories = new List<Category>();
            categories.Add(new BookCategory { Id = 1, Name = "Design", TotalPages = 400 });
            categories.Add(new VideoCategory { Id = 2, Name = "Movie", DurationInSeconds = 3400, SizeInBytes = 76003 });
            categories.Add(new BookCategory { Id = 3, Name = "Programming", TotalPages = 300 });
            categories.Add(new BookCategory { Id = 4, Name = "Project management", TotalPages = 600 });

            return Ok<IEnumerable<Category>>(categories);
            //return StatusCode(HttpStatusCode.NotImplemented);
        }

        // GET: odata/Categories(5)
        public IHttpActionResult GetCategory([FromODataUri] int key, ODataQueryOptions<Category> queryOptions)
        {
            // validate the query.
            try
            {
                queryOptions.Validate(_validationSettings);
            }
            catch (ODataException ex)
            {
                return BadRequest(ex.Message);
            }

            // return Ok<Category>(category);
            return StatusCode(HttpStatusCode.NotImplemented);
        }

        // PUT: odata/Categories(5)
        public IHttpActionResult Put([FromODataUri] int key, Delta<Category> delta)
        {
            return StatusCode(HttpStatusCode.MethodNotAllowed);
        }

        // POST: odata/Categories
        public IHttpActionResult Post(Category category)
        {
            return StatusCode(HttpStatusCode.MethodNotAllowed);
        }

        // PATCH: odata/Categories(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] int key, Delta<Category> delta)
        {
            return StatusCode(HttpStatusCode.MethodNotAllowed);
        }

        // DELETE: odata/Categories(5)
        public IHttpActionResult Delete([FromODataUri] int key)
        {
            return StatusCode(HttpStatusCode.MethodNotAllowed);
        }
    }
}
