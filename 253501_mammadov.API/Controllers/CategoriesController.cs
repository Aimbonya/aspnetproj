using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using _253501_mammadov.API.Data;
using mammadov.Domain.Entities;
using _253501_mammadov.API.Services;

namespace _253501_mammadov.API.Controllers
{
   

        [Route("api/[controller]")]
        [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly CategoryService _categoryService;

        public CategoriesController(AppDbContext context)
        {
            _context = context;
            _categoryService = new CategoryService(context);
        }

            // GET: api/Categories
            [HttpGet]
            public async Task<ActionResult<List<Category>>> GetCategories()
            {
                var categories = await _categoryService.GetCategoryListAsync();
                return Ok(categories);
            }

            // POST: api/Categories
            [HttpPost]
            public async Task<ActionResult<Category>> PostCategory(Category category)
            {
                _context.Categories.Add(category);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetCategories", new { id = category.Id }, category);
            }

            // Проверка существования категории
            private bool CategoryExists(int id)
            {
                return _context.Categories.Any(e => e.Id == id);
            }
        }
    
}
