using BallastLaneApplication.Domain.Entities;

namespace BallastLaneApplication.Domain.DTOs
{
    public class ProductDTO
    {
        public string? Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }


        public void Update(Product product, string userId)
        {
            product.Name = Name;
            product.Category = Category;
            product.Summary = Summary;
            product.Description = Description;
            product.Price = Price;
            product.UserId = userId;

            product.Modified = DateTime.Now;
            product.ModifiedBy = userId;
        }
    }
}
