using System.ComponentModel.DataAnnotations;

namespace SmartGrocery.Models
{
    public class ShoppingList
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }



        [Required, StringLength(100)]
        public string Name { get; set; } = string.Empty;

        // Навигационно свойство
        public ICollection<ShoppingListItem> Items { get; set; } = new List<ShoppingListItem>();
    }
}
