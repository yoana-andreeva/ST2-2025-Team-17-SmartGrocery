using SmartGrocery.Models;
using System.ComponentModel.DataAnnotations;

namespace SmartGrocery.DTOs
{
    public class ShopingListCreateDTO
    {
        [Required]
        public string Name { get; set; }

        // Product and quantity pairs
        public List<ProductSelection> SelectedProducts { get; set; } = new();

        // All available products for dropdown
        public List<Product> AvailableProducts { get; set; } = new();
    }

    public class ProductSelection
    {
        public int ProductId { get; set; }

        [Range(1, 999, ErrorMessage = "Quantity must be between 1 and 999.")]
        public int Quantity { get; set; } = 1;
    }
}
