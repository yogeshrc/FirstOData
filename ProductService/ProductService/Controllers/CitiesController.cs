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
    builder.EntitySet<City>("Cities");
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class CitiesController : ODataController
    {
        private static ODataValidationSettings _validationSettings = new ODataValidationSettings();

        // GET: odata/Cities
        public IHttpActionResult GetCities(ODataQueryOptions<City> queryOptions)
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

            // return Ok<IEnumerable<City>>(cities);
            return StatusCode(HttpStatusCode.NotImplemented);
        }

        // GET: odata/Cities(5)
        public IHttpActionResult GetCity([FromODataUri] int key, ODataQueryOptions<City> queryOptions)
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

            // return Ok<City>(city);
            return StatusCode(HttpStatusCode.NotImplemented);
        }

        // PUT: odata/Cities(5)
        public IHttpActionResult Put([FromODataUri] int key, Delta<City> delta)
        {
            Validate(delta.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // TODO: Get the entity here.

            // delta.Put(city);

            // TODO: Save the patched entity.

            // return Updated(city);
            return StatusCode(HttpStatusCode.NotImplemented);
        }

        // POST: odata/Cities
        public IHttpActionResult Post(City city)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // TODO: Add create logic here.

            // return Created(city);
            return StatusCode(HttpStatusCode.NotImplemented);
        }

        // PATCH: odata/Cities(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] int key, Delta<City> delta)
        {
            Validate(delta.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // TODO: Get the entity here.

            // delta.Patch(city);

            // TODO: Save the patched entity.

            // return Updated(city);
            return StatusCode(HttpStatusCode.NotImplemented);
        }

        // DELETE: odata/Cities(5)
        public IHttpActionResult Delete([FromODataUri] int key)
        {
            // TODO: Add delete logic here.

            // return StatusCode(HttpStatusCode.NoContent);
            return StatusCode(HttpStatusCode.NotImplemented);
        }
    }
}
