using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartGrocery.Models;
using SmartGrocery.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;  // For List


namespace SmartGrocery.Controllers
{
    [Authorize]
    public class AIController : Controller
    {
        private readonly DatabaseContext _context;
        private readonly AiService _aiService;
        private readonly UserManager<ApplicationUser> _userManager;

        public AIController(DatabaseContext context, AiService aiService, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _aiService = aiService;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Challenge();

            var lists = await _context.ShoppingLists
                .Where(l => l.UserId == user.Id)
                .ToListAsync();

            ViewBag.UserLists = lists;
            return View();
        }

        public class SuggestRequest
        {
            public int ShoppingListId { get; set; }
        }

        // ... (rest of the file remains the same)

        [HttpPost]
        public async Task<IActionResult> SuggestRecipes([FromBody] SuggestRequest req)
        {
            if (req == null || req.ShoppingListId <= 0)
                return BadRequest(new { success = false, message = "Invalid request." });

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Challenge();

            var list = await _context.ShoppingLists
                .Include(l => l.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(l => l.Id == req.ShoppingListId && l.UserId == user.Id);

            if (list == null)
                return NotFound(new { success = false, message = "Shopping list not found." });

            // Ensure list.Items is not null before processing
            if (list.Items == null)
                return Ok(new { success = false, message = "Shopping list has no items." });

            var products = list.Items
                .Select(i => i.Product?.Name ?? "Unknown")  // Safe null handling
                .Where(n => !string.IsNullOrWhiteSpace(n))
                .ToList();

            if (!products.Any())
                return Ok(new { success = false, message = "Shopping list is empty." });

            // This line should work now; if not, try explicit typing
            string productText = string.Join(", ", products);  // Changed to explicit string for clarity

            var prompt = $"You are a chef. Using only these ingredients: {productText}, suggest exactly 2 very short recipes. Output exactly in this format with no extra text, intros, or lists:\n\n1. Recipe Name: <name>\nInstruction: < very brief steps>\n\n2. Recipe Name: <name>\nInstruction: <brief steps>\n\nExample:\n1. Recipe Name: Simple Omelette\nInstruction: Beat eggs, cook in pan.\n\n2. Recipe Name: Bread Toast\nInstruction: Toast bread, add toppings.";

            try
            {
                var aiResponse = await _aiService.AskAsync(prompt);
                Console.WriteLine($"AI Response: {aiResponse}");
                return Ok(new { success = true, response = aiResponse });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "AI error: " + ex.Message });
            }
        
    }
}
}