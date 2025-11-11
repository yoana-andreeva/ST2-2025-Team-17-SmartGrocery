using System.ComponentModel.DataAnnotations;

namespace SmartGrocery.Models
{
    public class Recipe
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Title { get; set; } = string.Empty;

        [StringLength(200)]
        public string Description { get; set; } = string.Empty;

        [StringLength(500)]
        public string Ingredients { get; set; } = string.Empty;

        [StringLength(2000)]
        public string Instructions { get; set; } = string.Empty;

        public ICollection<Product> Products { get; set; } = new List<Product>();

    }
}
