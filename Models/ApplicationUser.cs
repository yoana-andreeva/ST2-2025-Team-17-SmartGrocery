using Microsoft.AspNetCore.Identity;

namespace SmartGrocery.Models
{
    public class ApplicationUser : IdentityUser
    {
        // Можеш да добавиш персонални полета
        public string? FullName { get; set; }

        // Един потребител може да има много списъци
        public ICollection<ShoppingList> ShoppingLists { get; set; } = new List<ShoppingList>();
    }
}
