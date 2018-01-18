using System.Web.OData.Builder;

namespace ProductService.Models
{
    public enum BookCoverTypes
    {
        Paperback,
        HardCover
    }

    public enum MediaFormat
    {
        Mp4,
        Wmv
    }

    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [Singleton]
        public Lookup Lookup { get; set; }
    }

    public class BookCategory: Category
    {
        public bool HasMultipleAuthors { get; set; }
        public BookCoverTypes CoverType { get; set; }
    }

    public class VideoCategory: Category
    {
        public MediaFormat VideoFormat { get; set; }
    }
}