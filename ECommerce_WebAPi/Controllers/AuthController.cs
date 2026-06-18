using ECommerce_WebAPi.Data;
using ECommerce_WebAPi.DTOs;
using ECommerce_WebAPi.Models;
using ECommerce_WebAPi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECommerce_WebAPi.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly EcomDbContext _context;
        private readonly JwtService _jwtService;
        public AuthController(EcomDbContext context, JwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            if (await _context.Users.AnyAsync(x => x.Email == dto.Email))
            {
                return BadRequest("Email already exists");
            }

            var user = new User
            {
                FullName = dto.FullName,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Role = "Customer"
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return Ok("User registered successfully");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var user = await _context.Users
                                .FirstOrDefaultAsync(
                                  x => x.Email == dto.Email);

            if (user == null)
            {
                return Unauthorized();
            }

            bool isValid = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);
            if (!isValid)
            {
                return Unauthorized();
            }

            var token = _jwtService.GenerateToken(user);

            return Ok(new
            {
                Token = token,
                message = "Welcome, You are Login."
            });
        }
    
}
}
