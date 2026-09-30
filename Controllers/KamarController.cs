// Controllers/KamarController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using App.Data;
using App.Models;
using App.Models.Dto;

namespace App.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class KamarController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public KamarController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<KamarResponseDto>>> GetAll()
        {
            var data = await _context.Kamars
                .Include(k => k.Gedung)
                .Include(k => k.Penghuni)
                .Select(k => new KamarResponseDto
                {
                    NoKamar = k.NoKamar,
                    Lantai = k.Lantai,
                    NamaGedung = k.Gedung!.NamaGedung,
                    JumlahPenghuni = k.Penghuni!.Count
                })
                .ToListAsync();

            return Ok(data);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Create(KamarCreateDto dto)
        {
            var gedungAda = await _context.Gedungs.AnyAsync(g => g.IdGedung == dto.IdGedung);
            if (!gedungAda) return BadRequest(new { message = "Gedung tidak ditemukan" });

            var kamar = new Kamar { Lantai = dto.Lantai, IdGedung = dto.IdGedung };
            _context.Kamars.Add(kamar);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Kamar ditambahkan", noKamar = kamar.NoKamar });
        }

        [HttpDelete("{noKamar}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int noKamar)
        {
            var kamar = await _context.Kamars.FindAsync(noKamar);
            if (kamar == null) return NotFound();

            var adaPenghuni = await _context.Users.AnyAsync(u => u.NoKamar == noKamar);
            if (adaPenghuni) return BadRequest(new { message = "Masih ada penghuni di kamar ini" });

            _context.Kamars.Remove(kamar);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Kamar dihapus" });
        }
    }
}