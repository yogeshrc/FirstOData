using ProductService.Models;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.OData;

namespace ProductService.Controllers
{
    public class LookupsController : ODataController
    {
        private static Lookup _singleton = null;

        public LookupsController()
        {
            if (_singleton == null) InitializeLookups();
        }

        private void InitializeLookups()
        {
            _singleton = new Lookup();
            _singleton.Cities = new List<City>
                    {
                        new City { Id = 1, Name = "Pune", Lookup = _singleton },
                        new City { Id = 2, Name = "Mumbai", Lookup = _singleton}
                    };

            _singleton.Categories = new List<Category>
            {
                new BookCategory {Id = 1, Name = "Design", Lookup = _singleton, CoverType = BookCoverTypes.HardCover, HasMultipleAuthors = true},
                new BookCategory {Id = 2, Name = "Design", Lookup = _singleton, CoverType = BookCoverTypes.HardCover, HasMultipleAuthors = false},
                new BookCategory {Id = 3, Name = "Design", Lookup = _singleton, CoverType = BookCoverTypes.Paperback, HasMultipleAuthors = true},

                new VideoCategory {Id = 4, Name = "Webinar", Lookup = _singleton, VideoFormat = MediaFormat.Wmv },
                new VideoCategory {Id = 5, Name = "Tutorial", Lookup = _singleton, VideoFormat = MediaFormat.Wmv },
                new VideoCategory {Id = 6, Name = "Movies", Lookup = _singleton, VideoFormat = MediaFormat.Wmv }
            };
        }

        public IHttpActionResult GetLookups()
        {
            return Ok(_singleton);
        }

        public IHttpActionResult GetCities()
        {
            return Ok(_singleton.Cities);
        }

        public IHttpActionResult GetCategories()
        {
            return Ok(_singleton.Categories);
        }
    }
}