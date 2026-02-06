namespace Ecommerce.Domain.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string? SKU { get; set; } 
        public string? Name { get; set; } 
        public string? Slug { get; set; }
        public string? Description { get; set; }
        public string? MetaTitle { get; set; }
        public string? MetaDescription { get; set; }
        public decimal Price { get; set; }
        public decimal? DiscountPrice { get; set; }
        public int Stock { get; set; }
        public decimal? Weight { get; set; }
        public string Status { get; set; } = "Active";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public Category? Category { get; set; }
        public Brand? Brand { get; set; }
        public UserCreated? CreatedBy { get; set; }
    }
}
