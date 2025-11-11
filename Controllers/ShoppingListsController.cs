using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartGrocery;
using SmartGrocery.DTOs;
using SmartGrocery.Models;
using System.Linq;
using System.Threading.Tasks;

namespace SmartGrocery.Controllers
{
    [Authorize]
    public class ShoppingListsController : Controller
    {
        private readonly DatabaseContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ShoppingListsController(DatabaseContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // ✅ GET: ShoppingLists
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Challenge();

            var userLists = await _context.ShoppingLists
                .Where(l => l.UserId == user.Id)
                .Include(l => l.Items)
                .ThenInclude(i => i.Product)
                .ToListAsync();

            return View(userLists);
        }

        // ✅ GET: ShoppingLists/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Challenge();

            var shoppingList = await _context.ShoppingLists
                .Include(l => l.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(m => m.Id == id && m.UserId == user.Id);

            if (shoppingList == null)
                return NotFound();

            return View(shoppingList);
        }

        // ✅ GET: ShoppingLists/Create
        public IActionResult Create()
        {
            var viewModel = new ShopingListCreateDTO
            {
                AvailableProducts = _context.Products.ToList()
            };

            return View(viewModel);
        }

        // ✅ POST: ShoppingLists/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ShopingListCreateDTO model)
        {
            if (!ModelState.IsValid)
            {
                model.AvailableProducts = await _context.Products.ToListAsync();
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Challenge();

            var shoppingList = new ShoppingList
            {
                Name = model.Name,
                UserId = user.Id,
                Items = model.SelectedProducts
                    .Where(p => p.ProductId != 0)
                    .Select(p => new ShoppingListItem
                    {
                        ProductId = p.ProductId,
                        Quantity = p.Quantity
                    })
                    .ToList()
            };

            _context.ShoppingLists.Add(shoppingList);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Shopping list created successfully!";
            return RedirectToAction(nameof(Index));
        }

        // ✅ GET: ShoppingLists/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Challenge();

            var shoppingList = await _context.ShoppingLists
                .Include(s => s.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(s => s.Id == id && s.UserId == user.Id);

            if (shoppingList == null)
                return NotFound();

            var model = new ShopingListCreateDTO
            {
                Name = shoppingList.Name,
                AvailableProducts = await _context.Products.ToListAsync(),
                SelectedProducts = shoppingList.Items
                    .Select(i => new ProductSelection
                    {
                        ProductId = i.ProductId,
                        Quantity = i.Quantity
                    })
                    .ToList()
            };

            ViewBag.ListId = shoppingList.Id;
            return View(model);
        }

        // ✅ POST: ShoppingLists/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ShopingListCreateDTO model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Challenge();

            var shoppingList = await _context.ShoppingLists
                .Include(s => s.Items)
                .FirstOrDefaultAsync(s => s.Id == id && s.UserId == user.Id);

            if (shoppingList == null)
                return NotFound();

            if (!ModelState.IsValid)
            {
                model.AvailableProducts = await _context.Products.ToListAsync();
                ViewBag.ListId = id;
                return View(model);
            }

            // Update list name
            shoppingList.Name = model.Name;

            // Remove old items
            _context.ShoppingListItems.RemoveRange(shoppingList.Items);

            // Add updated items
            shoppingList.Items = model.SelectedProducts
                .Where(p => p.ProductId != 0)
                .Select(p => new ShoppingListItem
                {
                    ProductId = p.ProductId,
                    Quantity = p.Quantity
                })
                .ToList();

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Shopping list updated successfully!";
            return RedirectToAction(nameof(Index));
        }

        // ✅ GET: ShoppingLists/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Challenge();

            var shoppingList = await _context.ShoppingLists
                .Include(l => l.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(m => m.Id == id && m.UserId == user.Id);

            if (shoppingList == null)
                return NotFound();

            return View(shoppingList);
        }

        // ✅ POST: ShoppingLists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Challenge();

            var shoppingList = await _context.ShoppingLists
                .FirstOrDefaultAsync(l => l.Id == id && l.UserId == user.Id);

            if (shoppingList != null)
            {
                _context.ShoppingLists.Remove(shoppingList);
                await _context.SaveChangesAsync();
            }

            TempData["SuccessMessage"] = "Shopping list deleted successfully!";
            return RedirectToAction(nameof(Index));
        }

        private bool ShoppingListExists(int id)
        {
            return _context.ShoppingLists.Any(e => e.Id == id);
        }
    }
}
