using ECommerce_WebAPi.Data;
using ECommerce_WebAPi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ECommerce_WebAPi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly EcomDbContext _context;

        public OrderController(EcomDbContext context)
        {
            _context = context;
        }

        [HttpPost("Checkout")]
        public async Task<IActionResult> Checkout()
        {
            var userId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier)!
                .Value);

            var cart = await _context.Carts
                .Include(x => x.CartItems)
                .ThenInclude(x => x.Product)
                .FirstOrDefaultAsync(x =>
                    x.UserId == userId);

            if (cart == null || !cart.CartItems.Any())
            {
                return BadRequest("Cart Empty");
            }

            var order = new Order
            {
                UserId = userId,
                TotalAmount = cart.CartItems.Sum(
                    x => x.Product.Price * x.Quantity),
                Status = "Pending"
            };

            _context.Orders.Add(order);

            await _context.SaveChangesAsync();

            foreach (var item in cart.CartItems)
            {
                _context.OrderItems.Add(new OrderItem
                {
                    OrderId = order.Id,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.Product.Price
                });

                item.Product.StockQuantity -= item.Quantity;
            }

            _context.CartItems.RemoveRange(
                cart.CartItems);

            await _context.SaveChangesAsync();

            return Ok(new
            {
                Message = "Order Placed",
                OrderId = order.Id
            });
        }

        [HttpGet("MyOrders")]
        public async Task<IActionResult> MyOrders()
        {
            var userId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier)!
                .Value);

            var orders = await _context.Orders
                .Where(x => x.UserId == userId)
                .Include(x => x.OrderItems)
                .ToListAsync();

            return Ok(orders);
        }

        [HttpGet("GetOrder")]
        public async Task<IActionResult>GetOrder(int id)
        {
            var order = await _context.Orders
                .Include(x => x.OrderItems)
                .ThenInclude(x => x.Product)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (order == null)
                return NotFound();

            return Ok(order);
        }

        [HttpPut("Status")]
        public async Task<IActionResult>UpdateStatus(int orderId, string status)
        {
            var order =
                await _context.Orders.FindAsync(orderId);

            if (order == null)
                return NotFound();

            order.Status = status;

            await _context.SaveChangesAsync();

            return Ok(order);
        }
    }
}
