using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartGrocery.Models
{
    public class ShoppingListItem
    {
        public int Id { get; set; }

        [Required]
        public int ShoppingListId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Range(0, 10000)]
        public int Quantity { get; set; }

        // Навигационни свойства
        [ForeignKey("ShoppingListId")]
        public ShoppingList? ShoppingList { get; set; }

        [ForeignKey("ProductId")]
        public Product? Product { get; set; }
    }
}
