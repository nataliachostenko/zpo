using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using wydatki2.Models;

namespace wydatki2.Controllers
{
    // dziedziczenie po klasie Controller
    public class CategoryController : Controller
    {
        //hermetyzacja
        private readonly ApplicationDbContext _context;

        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Category
        public async Task<IActionResult> Index()
        {
            return _context.Categories != null ?
                        View(await _context.Categories.ToListAsync()) :
                        Problem("Zbiór 'ApplicationDbContext.Categories'  jest pusty.");
        }


        // GET: Category/AddOrEdit
        public IActionResult AddOrEdit(int id = 0)
        {
            if (id == 0)
                return View(new Category());
            else
                return View(_context.Categories.Find(id));

        }

        // POST: Category/AddOrEdit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit([Bind("CategoryId,Title,Icon,Type")] Category category)
        {
            if (ModelState.IsValid)
            {
                if (category.CategoryId == 0)
                {
                    _context.Add(category);
                }
                else
                {
                    var existingCategory = await _context.Categories.FindAsync(category.CategoryId);
                    if (existingCategory == null)
                    {
                        return NotFound($"Kategoria o ID {category.CategoryId} nie została znaleziona.");
                    }

                    // Aktualizuj istniejącą kategorię
                    existingCategory.Title = category.Title;
                    existingCategory.Icon = category.Icon;
                    existingCategory.Type = category.Type;
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }



        //wyjatek
        public class CategoryNotFoundException : Exception
        {
            public CategoryNotFoundException(int categoryId)
                : base($"Kategoria o ID {categoryId} nie została znaleziona.")
            {
            }
        }

        // Generyczna metoda do pobierania rekordów 
        public async Task<IActionResult> GetAll<T>() where T : class
        {
            var entities = await _context.Set<T>().ToListAsync();
            return View(entities);
        }

        public async Task<IActionResult> Idx()
        {
            var categories = await _context.Set<Category>().ToListAsync(); 
            return View("Index", categories); // uzycie Index do widoku kategorii
        }




        // POST: Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Categories == null)
            {
                return Problem("Zbiór 'ApplicationDbContext.Categories' jest pusty.");
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                throw new CategoryNotFoundException(id);
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


    }
}