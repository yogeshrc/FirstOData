using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.OData.Builder;

namespace ProductService.Models
{
    public class Lookup
    {
        [Key]
        public Guid Id { get; set; }
        [AutoExpand] //DOESN'T WORK ON SINGLETON??
        public IEnumerable<City> Cities { get; set; }
        [AutoExpand] //DOESN'T WORK ON SINGLETON??
        public IEnumerable<Category> Categories { get; set; }
    }
}