using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.OData.Builder;

namespace ProductService.Models
{
    public class City
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [Singleton]
        public Lookup Lookup { get; set; }
    }
}