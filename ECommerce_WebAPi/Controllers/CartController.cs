using ECommerce_WebAPi.Data;
using ECommerce_WebAPi.DTOs;
using ECommerce_WebAPi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ECommerce_WebAPi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly EcomDbContext _context;
        public CartController(EcomDbContext context)
        {
            _context = context;
        }

        [HttpPost("AddToCart")]
        public async Task<IActionResult> AddToCart(AddToCartDto dto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
            {
                return Unauthorized("UserId claim not found");
            }

            var userId = int.Parse(userIdClaim.Value);

            var cart =
                   await _context.Carts.FirstOrDefaultAsync(x => x.UserId == userId);

            if (cart == null)
            {
                cart = new Models.Cart
                {
                    UserId = userId
                };
                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
            }

            var item =
                await _context.CartItems.FirstOrDefaultAsync(x => x.CartId == cart.Id && x.ProductId == dto.ProductId);

            if (item != null)
            {
                item.Quantity += dto.Quantity;

            }
            else
            {
                _context.CartItems.Add(new CartItem
                {
                    CartId = cart.Id,
                    ProductId = dto.ProductId,
                    Quantity = dto.Quantity
                });
                
            }

            await _context.SaveChangesAsync();
            return Ok("Added to Cart");
        }

        [HttpGet("MyCart")]
        public async Task<IActionResult> MyCart()
        {
            var userId =
                   int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var cart =
                   await _context.Carts.Include(x => x.CartItems).ThenInclude(x => x.Product)
                   .FirstOrDefaultAsync(x => x.UserId == userId);
            if(cart == null)
            {
                return Ok(new List<object>());
            }

            var result = cart.CartItems.Select(x => new
            {
                ProductId = x.ProductId,
                ProductName = x.Product.Name,
                Price = x.Product.Price,
                Quantity = x.Quantity,
                Total = x.Product.Price * x.Quantity
            });
            return Ok(result);
        }

        [HttpDelete("Remove")]
        public async Task<IActionResult> Remove(int productId)
        {
            var item = await _context.CartItems
                .FirstOrDefaultAsync(x =>
                    x.ProductId == productId);

            if (item == null)
                return NotFound();

            _context.CartItems.Remove(item);

            await _context.SaveChangesAsync();

            return Ok("Removed");
        }

        [HttpPut("UpdateQuantity")]
        public async Task<IActionResult>UpdateQuantity(int productId, int quantity)
        {
            var item = await _context.CartItems
                .FirstOrDefaultAsync(x =>
                    x.ProductId == productId);

            if (item == null)
                return NotFound();

            item.Quantity = quantity;

            await _context.SaveChangesAsync();

            return Ok(item);
        }
    }
}
