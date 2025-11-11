using System.ComponentModel.DataAnnotations;

namespace SmartGrocery.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Range(0, 10000)]
        public double Quantity { get; set; }

        [Required, StringLength(20)]
        public string Unit { get; set; } = "pcs";

        [Range(0, 10000)]
        public decimal Price { get; set; }

        public ICollection<Recipe> Recipes { get; set; } = new List<Recipe>();

    }
}

