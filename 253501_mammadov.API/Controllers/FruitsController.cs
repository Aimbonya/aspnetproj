using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using _253501_mammadov.API.Data;
using _253501_mammadov.API.Models;
using mammadov.Domain.Entities;
using _253501_mammadov.API.Services;
using Microsoft.AspNetCore.Authorization;

namespace _253501_mammadov.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FruitsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IFruitService _fruitService;

        public FruitsController(AppDbContext context, IFruitService fruitService)
        {
            _context = context;
            _fruitService = fruitService;
        }

        // GET: api/Fruits
        [HttpGet("{category?}")]
        [AllowAnonymous]
        public async Task<ActionResult<ResponseData<List<Fruit>>>> GetFruits(
        string? category,
        int pageNo = 1,
        int pageSize = 3)
        {
            return Ok(await _fruitService.GetProductListAsync(
            category,
            pageNo,
            pageSize));
        }

        [HttpDelete("{id:int}")]
        [Authorize(Policy = "admin")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            Console.WriteLine("im in delete");
            var result = await _fruitService.DeleteProductAsync(id);

            if (!result.Successfull)
            {
                return NotFound(result.ErrorMessage); // Возвращаем 404, если продукт не найден
            }

            return Ok(new { Message = result.ErrorMessage });
        }

        // GET: api/products/{id}
        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetProductById(int id)
        {
            Console.WriteLine("im in getbyid");
            if (id <= 0)
            {
                return BadRequest("Invalid product ID.");
            }

            var product = await _fruitService.GetProductByIdAsync(id);

            if (product == null)
            {
                return NotFound($"Product with ID {id} not found.");
            }

            return Ok(product);
        }

        [HttpPost("api/products")]
        [Authorize(Policy = "admin")]
        public async Task<IActionResult> CreateProduct([FromForm] Fruit product, IFormFile? imageFile)
        {

            Console.WriteLine("im in create");
            var result = await _fruitService.CreateProductAsync(product, imageFile);

            if (!result.Successfull)
            {
                return BadRequest(result.ErrorMessage);
            }

            return Ok(result.Data);
        }

        // POST: api/Fruits
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Policy = "admin")]
        public async Task<ActionResult<Fruit>> PostFruit(Fruit fruit)
        {
            Console.WriteLine("im in postfruit");
            _context.Fruits.Add(fruit);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProductById", new { id = fruit.Id }, fruit);
        }

        private bool FruitExists(int id)
        {
            return _context.Fruits.Any(e => e.Id == id);
        }

        [HttpPut("{id:int}")]
        [Authorize(Policy = "admin")]
        public async Task<IActionResult> UpdateProduct(int id, Fruit product)
        {
            Console.WriteLine("im in update");
            var result = await _fruitService.UpdateProductAsync(id, product);

            if (!result.Successfull)
            {
                return BadRequest(new { Message = result.ErrorMessage });
            }

            return Ok(result.Data);
        }

    }
}
