// Controllers/AuthController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using App.Data;
using App.Models;
using App.Models.Dto;
using App.Services;

namespace App.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly JwtService _jwtService;

        public AuthController(ApplicationDbContext context, JwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDto>> Login(LoginRequestDto request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                return Unauthorized(new { message = "Email atau password salah" });

            var token = _jwtService.GenerateToken(user);

            return Ok(new LoginResponseDto
            {
                Token = token,
                Nama = user.Nama,
                Role = user.Role.ToString()
            });
        }

        [HttpPost("register")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Register(RegisterRequestDto request)
        {
            if (await _context.Users.AnyAsync(u => u.Email == request.Email))
                return BadRequest(new { message = "Email sudah terdaftar" });

            if (!Enum.TryParse<Role>(request.Role, true, out var role))
                return BadRequest(new { message = "Role tidak valid" });

            var user = new User
            {
                Nama = request.Nama,
                Nim = request.Nim,
                Asal = request.Asal,
                JenisKelamin = request.JenisKelamin,
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Role = role,
                NoKamar = request.NoKamar
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Akun berhasil dibuat", idUser = user.IdUser });
        }
    }
}