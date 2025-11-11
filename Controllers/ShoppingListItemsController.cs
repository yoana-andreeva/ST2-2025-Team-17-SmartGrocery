using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SmartGrocery.Models;

namespace SmartGrocery.Controllers
{
    public class ShoppingListItemsController : Controller
    {
        private readonly DatabaseContext _context;

        public ShoppingListItemsController(DatabaseContext context)
        {
            _context = context;
        }

        // ✅ GET: ShoppingListItems/Create?shoppingListId=5
        public IActionResult Create(int shoppingListId)
        {
            // Проверка дали списъкът съществува
            var list = _context.ShoppingLists.Find(shoppingListId);
            if (list == null)
            {
                return NotFound("Shopping list not found.");
            }

            ViewBag.ProductId = new SelectList(_context.Products, "Id", "Name");
            ViewBag.SelectedShoppingListId = shoppingListId; // ще се ползва във view-то

            return View();
        }

        // ✅ POST: ShoppingListItems/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ShoppingListId,ProductId,Quantity")] ShoppingListItem item)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ProductId = new SelectList(_context.Products, "Id", "Name", item.ProductId);
                ViewBag.SelectedShoppingListId = item.ShoppingListId;
                return View(item);
            }

            _context.ShoppingListItems.Add(item);
            await _context.SaveChangesAsync();

            // След добавяне → връщаме се към детайлите на конкретния списък
            return RedirectToAction("Details", "ShoppingLists", new { id = item.ShoppingListId });
        }
    }
}
