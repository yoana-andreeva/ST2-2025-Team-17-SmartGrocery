using Microsoft.AspNetCore.Mvc;
using SmartGrocery.Models;
using SmartGrocery.Repositories;

namespace SmartGrocery.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IGenericRepository<Product> _repository;

        // Внедряване на зависимост чрез конструктора
        public ProductsController(IGenericRepository<Product> repository)
        {
            _repository = repository;
        }

        // READ - Извеждане на всички продукти
        public async Task<IActionResult> Index()
        {
            var products = await _repository.GetAllAsync();
            return View(products);
        }

        // READ - Детайли за един продукт
        public async Task<IActionResult> Details(int id)
        {
            var product = await _repository.GetByIdAsync(id);
            if (product == null) return NotFound();
            return View(product);
        }

        // CREATE - форма
        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            if (ModelState.IsValid)
            {
                await _repository.AddAsync(product);
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // UPDATE - форма
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _repository.GetByIdAsync(id);
            if (product == null) return NotFound();
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product)
        {
            if (id != product.Id) return NotFound();
            if (ModelState.IsValid)
            {
                await _repository.UpdateAsync(product);
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // DELETE - потвърждение
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _repository.GetByIdAsync(id);
            if (product == null) return NotFound();
            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _repository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}

//brand new code for products controller because of the repositories