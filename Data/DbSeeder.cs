using SmartGrocery.Models;

namespace SmartGrocery.Data
{
    public static class DbSeeder
    {
        public static void Seed(DatabaseContext context)
        {
            if (!context.Products.Any())
            {
                context.Products.AddRange(
                    new Product { Name = "Apple", Price = 1.20M },
                    new Product { Name = "Milk", Price = 2.30M },
                    new Product { Name = "Bread", Price = 1.10M },
                    new Product { Name = "Chicken fillet", Price = 8.50M }
                );
                context.SaveChanges();
            }

            if (!context.Recipes.Any())
            {
                context.Recipes.AddRange(
                    new Recipe { Title = "Salad" },
                    new Recipe { Title = "Chicken with rice" },
                    new Recipe { Title = "Pancakes" }
                );
                context.SaveChanges();
            }
        }
    }
}
