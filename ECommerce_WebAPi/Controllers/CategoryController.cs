using ECommerce_WebAPi.Data;
using ECommerce_WebAPi.DTOs;
using ECommerce_WebAPi.Models;
using ECommerce_WebAPi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;

namespace ECommerce_WebAPi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly EcomDbContext _context;
        public CategoryController(EcomDbContext context)
        {
            _context = context;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(CreateCategoryDto dto)
        {
            var category = new Category
            {
                Name = dto.Name,
                Description = dto.Description
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return Ok(category);
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var categories =
                await _context.Categories.ToListAsync();

            return Ok(categories);
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(int id)
        {
            var category =
                await _context.Categories.FindAsync(id);

            if (category == null)
                return NotFound();

            return Ok(category);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(int id, CreateCategoryDto dto)
        {
            var category =
                await _context.Categories.FindAsync(id);

            if (category == null)
                return NotFound();

            category.Name = dto.Name;
            category.Description = dto.Description;

            await _context.SaveChangesAsync();
            return Ok(category);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var category =
                await _context.Categories.FindAsync(id);

            if (category == null)
                return NotFound();

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return Ok("Category deleted successfully");
        }
    }
}
