namespace Ecommerce.Domain.Entities
{
    public class ProductRaw
    {
        public int Id { get; set; }
        public string SKU { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string MetaTitle { get; set; } = string.Empty;
        public string MetaDescription { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public decimal? DiscountPrice { get; set; }
        public int Stock { get; set; }
        public decimal? Weight { get; set; }
        public string Status { get; set; } = "Active";
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public string CategoryName { get; set; } = string.Empty;
        public string CategorySlug { get; set; } = string.Empty;
        public string CategoryMetaTitle { get; set; } = string.Empty;
        public string CategoryDescription { get; set; } = string.Empty;

        public string BrandName { get; set; } = string.Empty;
        public string BrandSlug { get; set; } = string.Empty;
        public string BrandMetaTitle { get; set; } = string.Empty;
        public string BrandMetaDescription { get; set; } = string.Empty;

        public string CreatedByUserName { get; set; } = string.Empty;
    }

}
