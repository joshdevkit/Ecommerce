namespace Ecommerce.Domain.Entities
{
    public class Brand
    {
        public int BrandID { get; set; }
        public string BrandName { get; set; } = string.Empty;
        public string BrandSlug { get; set; } = string.Empty;
        public string BrandMetaTitle { get; set; } = string.Empty;
        public string BrandMetaDescription { get; set; } = string.Empty;
    }
}
