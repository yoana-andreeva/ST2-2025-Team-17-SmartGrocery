using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SmartGrocery.Models;

namespace SmartGrocery
{
    public class DatabaseContext : IdentityDbContext<ApplicationUser>
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<ShoppingList> ShoppingLists { get; set; }
        public DbSet<ShoppingListItem> ShoppingListItems { get; set; }
        public DbSet<Recipe> Recipes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Релации между списъци и потребител
           // modelBuilder.Entity<ShoppingList>()
              //  .HasOne<ApplicationUser>()
              // .WithMany(u => u.ShoppingLists)
               // .HasForeignKey("UserId")
               // .OnDelete(DeleteBehavior.Cascade);

          modelBuilder.Entity<ShoppingList>()
        .HasOne(sl => sl.User)
        .WithMany(u => u.ShoppingLists)
        .HasForeignKey(sl => sl.UserId)
        .OnDelete(DeleteBehavior.Cascade);

            // Останалите ти релации
            modelBuilder.Entity<ShoppingListItem>()
                .HasOne(i => i.ShoppingList)
                .WithMany(l => l.Items)
                .HasForeignKey(i => i.ShoppingListId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ShoppingListItem>()
                .HasOne(i => i.Product)
                .WithMany()
                .HasForeignKey(i => i.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Product>()
                .HasMany(p => p.Recipes)
                .WithMany(r => r.Products)
                .UsingEntity(j => j.ToTable("RecipeProducts"));
        }
    }
}
