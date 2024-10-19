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

namespace _253501_mammadov.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FruitsController : ControllerBase
    {
                private readonly AppDbContext _context;
        private FruitService _fruitService;

        public FruitsController(AppDbContext context)
        {
            _context = context;
            _fruitService = new FruitService(context);
        }

        // GET: api/Fruits
        [HttpGet("{category?}")]
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

        // POST: api/Fruits
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Fruit>> PostFruit(Fruit fruit)
        {
            _context.Fruits.Add(fruit);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFruit", new { id = fruit.Id }, fruit);
        }

        private bool FruitExists(int id)
        {
            return _context.Fruits.Any(e => e.Id == id);
        }
    }
}
