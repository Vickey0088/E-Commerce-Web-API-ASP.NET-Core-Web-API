using ECommerce_WebAPi.Data;
using ECommerce_WebAPi.DTOs;
using ECommerce_WebAPi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECommerce_WebAPi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly EcomDbContext _context;
        public ProductController(EcomDbContext context)
        {
            _context = context;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(CreateProductDto dto)
        {
            var category = await _context.Categories.FindAsync(dto.CategoryId);
            if (category == null)
            {
                return BadRequest("Invalid Category Id");
            }

            var product = new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                StockQuantity = dto.StockQuantity,
                ImageUrl = dto.ImageUrl,
                CategoryId = dto.CategoryId
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return Ok(product);
        }

        [HttpPost("GetAll")]
        public async Task<IActionResult> GetProducts([FromBody] ProductQueryDto query)
        {
            var products = _context.Products
                                 .Include(x=>x.Category)   
                                 .AsQueryable();
            
            // Search
            if (!string.IsNullOrEmpty(query.Search))
            {
                products = products.Where(x=>x.Name.Contains(query.Search));
            }

            // Filter by Category
            if (query.CategoryId.HasValue)
            {
                products = products.Where(x=>x.CategoryId == query.CategoryId);
            }

            // Sorting
            if (!string.IsNullOrEmpty(query.SortBy))
            {
                switch (query.SortBy.ToLower())
                {
                    case "price":

                        products = query.Descending
                            ? products.OrderByDescending(x => x.Price)
                            : products.OrderBy(x => x.Price);

                        break;

                    case "name":

                        products = query.Descending
                            ? products.OrderByDescending(x => x.Name)
                            : products.OrderBy(x => x.Name);

                        break;
                }
            }

            // Total Records
            var totalRecords = await products.CountAsync();

            if (query.Page <= 0)
                query.Page = 1;

            if (query.PageSize <= 0)
                query.PageSize = 10;

            // Pagination
            var result = await products
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.Price,
                    x.StockQuantity,
                    Category = x.Category.Name
                })
                .ToListAsync();

            return Ok(new
            {
                TotalRecords = totalRecords,
                Page = query.Page,
                PageSize = query.PageSize,
                Data = result
            });
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _context.Products
                                 .Include(x => x.Category)
                                 .FirstOrDefaultAsync(x => x.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpPost("Update")]
        public async Task<IActionResult> Update(int id, UpdateProductDto dto)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            var category = await _context.Categories.FindAsync(dto.CategoryId);
            if (category == null)
            {
                return BadRequest("Invalid Category Id");
            }
            product.Name = dto.Name;
            product.Description = dto.Description;
            product.Price = dto.Price;
            product.StockQuantity = dto.StockQuantity;
            product.ImageUrl = dto.ImageUrl;
            product.CategoryId = dto.CategoryId;

            await _context.SaveChangesAsync();
            return Ok(product);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
