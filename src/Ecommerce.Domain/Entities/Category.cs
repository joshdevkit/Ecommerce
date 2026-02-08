
namespace Ecommerce.Domain.Entities
{
    public class Category
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string CategorySlug { get; set; } = string.Empty;
        public string CategoryMetaTitle { get; set; } = string.Empty;
        public string CategoryDescription { get; set; } = string.Empty;
    }
}
